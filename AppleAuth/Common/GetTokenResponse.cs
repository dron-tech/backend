using System.Text.Json.Serialization;

namespace AppleAuth.Common;

public class GetTokenResponse
{
    [JsonPropertyName("id_token")] 
    public string IdToken { get; set; } = string.Empty;
}
