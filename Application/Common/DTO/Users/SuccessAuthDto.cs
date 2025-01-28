namespace Application.Common.DTO.Users;

public class SuccessAuthDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime AccessExpiryAt { get; set; }
    public DateTime RefreshExpiryAt { get; set; }
}
