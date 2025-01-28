using Application.Common.DTO.Profiles;

namespace Application.Common.DTO.Users;

public class UserDto
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsEmailConfirm { get; set; }
    public ProfileDto Profile { get; set; } = new();
    public PublicationStat PublicationStat { get; set; } = new();
}
