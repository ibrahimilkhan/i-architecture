using System;

namespace Core.Persistence.Repositories;

public class Entity<TId> : IEntityTimestamp
{
    public TId Id { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }


    public Entity(TId id)
    {
        Id = id;
    }

    public Entity()
    {
        Id = default;
    }
}