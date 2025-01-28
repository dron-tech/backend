namespace Application.Common.Configs;

public class JwtCfg
{
    public string Secret { get; set; } = string.Empty;
    public int AccessTokenExpiresInMin { get; set; }
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int RefreshTokenExpiresInDays { get; set; }
}
