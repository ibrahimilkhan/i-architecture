using System.Linq.Expressions;
using System.Reflection;
using Core.Persistence.Dynamic;
using Core.Persistence.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;

namespace Core.Persistence.Repositories;

public class EfRepositoryBase<TEntity, TEntityId, TContext> : IAsyncRepository<TEntity, TEntityId> where TEntity : Entity<TEntityId> where TContext : DbContext 
{
    protected readonly TContext Context;

    public EfRepositoryBase(TContext context)
    {
        Context = context;
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        entity.CreatedDate = DateTime.UtcNow;
        await Context.AddAsync(entity);
        await Context.SaveChangesAsync();

        return entity;
    }

    public async Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities)
    {
        foreach (var entity in entities)
            entity.CreatedDate = DateTime.UtcNow;

        await Context.AddRangeAsync(entities);
        await Context.SaveChangesAsync();

        return entities;
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query();

        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);

        return await queryable.AnyAsync(cancellationToken);
    }

    public async Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false)
    {
        await SetEntityAsDeletedAsync(entity, permanent);
        await Context.SaveChangesAsync();

        return entity;
    }

    public async Task<ICollection<TEntity>> DeleteRangeAsync(ICollection<TEntity> entities, bool permanent = false)
    {
        await SetEntitiesAsDeletedAsync(entities, permanent);
        await Context.SaveChangesAsync();

        return entities;
    }

    public async Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query();

        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);

        return await queryable.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<Paginate<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query();

        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        if (orderBy != null)
            return await orderBy(queryable).ToPaginateAsync(index, size, cancellationToken);

        return await queryable.ToPaginateAsync(index, size, cancellationToken);
    }

    public async Task<Paginate<TEntity>> GetListDynamicAsync(
        DynamicQuery dynamic,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query().ToDynamic(dynamic);

        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);

        return await queryable.ToPaginateAsync(index, size, cancellationToken);
    }

    public IQueryable<TEntity> Query()
    {
        throw new NotImplementedException();
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        entity.UpdatedDate = DateTime.UtcNow;
        Context.Update(entity);
        await Context.SaveChangesAsync();

        return entity;
    }

    public async Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entities)
    {
        foreach (var entity in entities)
            entity.UpdatedDate = DateTime.UtcNow;

        Context.UpdateRange(entities);

        await Context.SaveChangesAsync();
        return entities;
    }


    protected async Task SetEntityAsDeletedAsync(TEntity entity, bool permanent)
    {
        if (!permanent)
        {
            CheckHasEntityHaveOneToOneRelation(entity);
            await SetEntityAsSoftDeletedAsync(entity);
        }
        else
        {
            Context.Remove(entity);
        }
    }

    private async Task SetEntityAsSoftDeletedAsync(TEntity entity)
    {
        if (entity is not IEntityTimestamp timestampedEntity)
            throw new InvalidOperationException("Entity must implement IEntityTimestamp for soft delete.");

        //Ana entity'yi soft-delete olarak i�aretle
        timestampedEntity.DeletedDate = DateTime.UtcNow;
        Context.Update(entity);

        var entry = Context.Entry(entity);

        //Tekil navigation property'ler (HasOne gibi)
        var referenceNavigations = entry.Navigations
            .Where(n => !n.Metadata.IsCollection &&
                        n.Metadata.TargetEntityType.ClrType.GetInterfaces().Contains(typeof(IEntityTimestamp)))
            .ToList();

        foreach (var nav in referenceNavigations)
        {
            // Navigation de�eri zaten y�klenmi�se, kontrol et
            if (!nav.IsLoaded)
                await nav.LoadAsync();

            if (nav.CurrentValue is IEntityTimestamp relatedEntity && relatedEntity.DeletedDate == null)
            {
                relatedEntity.DeletedDate = DateTime.UtcNow;
                Context.Update(relatedEntity);
            }
        }

        //Koleksiyon navigation property'ler (HasMany gibi)
        var collectionNavigations = entry.Navigations
            .Where(n => n.Metadata.IsCollection)
            .ToList();

        foreach (var collectionNav in collectionNavigations)
        {
            if (!collectionNav.IsLoaded)
                await collectionNav.LoadAsync();

            if (collectionNav.CurrentValue is IEnumerable<object> relatedEntities)
            {
                foreach (var related in relatedEntities.OfType<IEntityTimestamp>())
                {
                    if (related.DeletedDate == null)
                    {
                        related.DeletedDate = DateTime.UtcNow;
                        Context.Update(related);
                    }
                }
            }
        }

        await Context.SaveChangesAsync();
    }

    protected void CheckHasEntityHaveOneToOneRelation(TEntity entity)
    {
        bool hasEntityHaveOneToOneRelation = Context.Entry(entity)
        .Metadata.GetForeignKeys()
        .All(
            x =>
            x.DependentToPrincipal?.IsCollection == true ||
            x.PrincipalToDependent?.IsCollection == true ||
            x.DependentToPrincipal?.ForeignKey.DeclaringEntityType.ClrType == entity.GetType()) == false;

        if (hasEntityHaveOneToOneRelation)
            throw new InvalidOperationException("Entity has one-to-one relation. Soft delete may cause problems if you try to create entry again by same foreign key");
    }

    protected IQueryable<object> GetRelationLoaderQuery(IQueryable query, Type navigationPropertyType)
    {
        Type queryProviderType = query.Provider.GetType();
        MethodInfo createQueryMethod =
            queryProviderType.GetMethods().
            First(x => x is { Name: nameof(query.Provider.CreateQuery), IsGenericMethod: true })?
            .MakeGenericMethod(navigationPropertyType)
            ?? throw new InvalidOperationException("Could not find CreateQuery method on query provider");

        var queryProviderQuery = (IQueryable<object>)createQueryMethod.Invoke(query.Provider, parameters: [query.Expression])!;

        return queryProviderQuery.Where(x => !((IEntityTimestamp)x).DeletedDate.HasValue);
    }

    protected async Task SetEntitiesAsDeletedAsync(ICollection<TEntity> entities, bool permanent)
    {
        foreach (var entity in entities)
            await SetEntityAsDeletedAsync(entity, permanent);
    }
}