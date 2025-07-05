using Core.Persistence.Repositories;

namespace Domain.Entities;

public class Fuel : Entity<Guid>
{
    public string Name { get; set; }

    public virtual ICollection<Model> Models { get; set; }

    public Fuel()
    {
        Name = string.Empty;
        Models = new HashSet<Model>();
    }

    public Fuel(string name) : this()
    {
        Name = name;
    }
}