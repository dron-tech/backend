using Domain.Common.Enums;

namespace Domain.Entities;

public class Profile : BaseEntity
{
    public string? Cover { get; set; }
    public string? Avatar { get; set; }
    public string? Name { get; set; }
    public UserStatus? Status { get; set; }
    public string? Desc { get; set; }
    public string? Link { get; set; }

    public User User { get; set; } = default!;
}
