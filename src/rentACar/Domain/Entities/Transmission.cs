using Core.Persistence.Repositories;

namespace Domain.Entities;

public class Transmission : Entity<Guid>
{
    public string Name { get; set; }

    public ICollection<Model> Models { get; set; }

    public Transmission()
    {
        Name = string.Empty;
        Models = new HashSet<Model>();
    }

    public Transmission(string name) : this()
    {
        Name = name;
    }
}