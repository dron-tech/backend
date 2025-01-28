using System.Text.Json.Serialization;

namespace ThirdPartyAuthService.Services.Facebook.Common.DTO;

public class AppTokenDto
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
}
