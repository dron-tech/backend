using Domain.Common.Enums;

namespace WebApp.Common.DTO.Profiles;

public class UpdateProfileDto
{
    public IFormFile? Cover { get; set; }
    public IFormFile? Avatar { get; set; }
    public string? Login { get; set; }

    public string? Name { get; set; }
    public string? Desc { get; set; }
    public UserStatus? Status { get; set; }

    public string? Link { get; set; }
}
