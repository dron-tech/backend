using System.Text.Json.Serialization;

namespace ThirdPartyAuthService.Services.Facebook.Common.DTO;

public class VerifyTokenData
{
    [JsonPropertyName("is_valid")]
    public bool IsValid { get; set; }
    
    [JsonPropertyName("scopes")]
    public string[] Scope { get; set; }
}
