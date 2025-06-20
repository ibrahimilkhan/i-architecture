using System;

namespace Core.Persistence.Repositories;

public class Entity<TId>
{
    public TId Id { get; set; }

    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedDate { get; set; } = DateTime.UtcNow;


    public Entity(TId id)
    {
        Id = id;
    }

    public Entity()
    {
        Id = default;
    }
}