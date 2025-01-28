using System.ComponentModel.DataAnnotations;

namespace WebApp.Common.DTO.Auth;

public class RefreshTokenDto
{
    [MinLength(32)]
    public string RefreshToken { get; set; } = string.Empty;

    [MinLength(32)]
    public string ExpiredAccessToken { get; set; } = string.Empty;
}
