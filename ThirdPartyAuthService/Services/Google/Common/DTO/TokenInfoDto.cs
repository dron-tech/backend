using System.Text.Json.Serialization;

namespace ThirdPartyAuthService.Services.Google.Common.DTO;

public class TokenInfoDto
{
    [JsonPropertyName("issued_to")]
    public string IssuedTo { get; set; }
    
    [JsonPropertyName("verified_email")]
    public bool VerifiedEmail { get; set; }
    
    public string Audience { get; set; }
    public string Scope { get; set; }
}
