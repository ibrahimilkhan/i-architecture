namespace Core.Persistence.Repositories;

public interface IEntityTimestamp
{
    DateTime CreatedDate { get; set; }
    DateTime? UpdatedDate { get; set; }
    DateTime? DeletedDate { get; set; }
}