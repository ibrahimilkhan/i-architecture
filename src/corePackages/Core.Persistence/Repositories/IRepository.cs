using System;
using System.Linq.Expressions;
using Core.Persistence.Paging;
using Microsoft.EntityFrameworkCore.Query;

namespace Core.Persistence.Repositories;

public interface IRepository<TEntity, TEntityId> : IQuery<TEntity> where TEntity : Entity<TEntityId>
{
    TEntity GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken cancellationToken = default);

    Task<Paginate<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default);

    Task<Paginate<TEntity>> GetListDynamicAsync(
        DynamicQuery dynamic,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    TEntity AddAsync(TEntity entity);
    Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities);

    TEntity UpdateAsync(TEntity entity);
    Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entities);

    TEntity DeleteAsync(TEntity entity, bool permanent = false);
    TEntity DeleteRangeAsync(ICollection<TEntity> entities, bool permanent = false);
}