using System.Collections.ObjectModel;

namespace Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; init; } = string.Empty;
    public string NormalizedName { get; init; } = string.Empty;

    public ICollection<User> Users { get; set; } = new Collection<User>();
}
