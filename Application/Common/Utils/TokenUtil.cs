using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Configs;
using Microsoft.IdentityModel.Tokens;

namespace Application.Common.Utils;

public static class TokenUtil
{
    public static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        
        return Convert.ToBase64String(randomNumber);
    }
    
    public static string GenerateAccessToken(int id, string role, bool isEmailConfirm, DateTime expires, JwtCfg cfg)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, id.ToString()),
            new Claim(ClaimTypes.Role, role),
            new Claim("EmailIsConfirm", isEmailConfirm.ToString()),
        };

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg.Secret));
        var token = new JwtSecurityToken(
            issuer: cfg.Issuer,
            audience: cfg.Audience,
            expires: expires,
            claims: claims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public static int GenerateCode()
    {
        var rnd = new Random(); 
        var value= rnd.Next(100000,999999);
        
        return value;
    }
}
