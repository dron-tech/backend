using Application.Common.Enums;

namespace ThirdPartyAuthService.Services.Google.Common;

public class GoogleCfg
{
    public GoogleClient[] Clients { get; set; }
    public string[] Scope { get; set; }
}

public class GoogleClient
{
    public ThirdPartyPlatform Platform { get; set; }
    public string ClientId { get; set; }
}
