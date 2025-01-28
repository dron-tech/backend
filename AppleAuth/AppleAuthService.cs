using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Security.Cryptography;
using AppleAuth.Common;
using AppleAuth.Common.Configs;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AppleAuth;

public class AppleAuthService : IAppleAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly HttpClient _httpClient;
    private readonly AppleCfg _cfg;
    private const string TokenUri = "https://appleid.apple.com/auth/token";

    public AppleAuthService(IUnitOfWork unitOfWork, HttpClient httpClient, IOptions<AppleCfg> opts)
    {
        _unitOfWork = unitOfWork;
        _httpClient = httpClient;
        _cfg = opts.Value;
    }

    public async Task<string> GetEmailByCodeOrFail(string code)
    {
        var data = new KeyValuePair<string, string>[]
        {
            new("client_id", _cfg.ClientId),
            new("code", code),
            new("client_secret", GenerateClientSecret()),
            new("grant_type", "authorization_code")
        };

        var response = await _httpClient.PostAsync(TokenUri, new FormUrlEncodedContent(data));
        if (!response.IsSuccessStatusCode)
        {
            throw new BadRequestException("Incorrect provided code");
        }
        
        var content = await response.Content.ReadFromJsonAsync<GetTokenResponse>();
        if (content is null)
        {
            throw new Exception($"Empty content\nResponse: {response}");
        }

        return await GetUserEmailByTokenOrFail(content.IdToken);
    }

    private string GenerateClientSecret()
    {
        var now = DateTimeOffset.UtcNow;
        var ecDsaCng = ECDsa.Create();

        ecDsaCng.ImportPkcs8PrivateKey(Convert.FromBase64String(_cfg.Key), out _);
        var key = new ECDsaSecurityKey(ecDsaCng)
        {
            KeyId = _cfg.KeyId
        };
        
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.EcdsaSha256);

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _cfg.TeamId,
            Subject = new ClaimsIdentity(new Claim[]
            {
                new("iss", _cfg.TeamId),
                new("iat", now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new("exp", now.AddDays(7).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new("aud", "https://appleid.apple.com"),
                new("sub", _cfg.ClientId)
            }),
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = signingCredentials,
        };
        
        var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    private async Task<string> GetUserEmailByTokenOrFail(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtSecToken = handler.ReadJwtToken(token);

        var sub = ParseSubOrFail(jwtSecToken);

        var user = await _unitOfWork.UserRepository.GetByAppleSub(sub);
        if (user is not null)
        {
            return user.Email;
        }

        var email = ParseVerifiedEmailOrFail(jwtSecToken);
        user = await _unitOfWork.UserRepository.GetByEmail(email);
        
        if (user is not null)
        {
            user.AppleSub = sub;
            await _unitOfWork.CommitAsync(new CancellationToken());

            return user.Email;
        }

        var newUser = await CreateUser(email, sub);
        
        await _unitOfWork.UserRepository.Insert(newUser);
        await _unitOfWork.CommitAsync(new CancellationToken());

        return newUser.Email;
    }

    private async Task<User> CreateUser(string email, string sub)
    {
        var role = await _unitOfWork.RoleRepository.GetUserRole();
        return new User
        {
            Email = email,
            Role = role,
            IsEmailConfirm = true,
            AppleSub = sub
        };
    }
    
    private static string ParseSubOrFail(JwtSecurityToken token)
    {
        var sub = token.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        if (sub is null)
        {
            throw new Exception($"Sub equal null\nToken:{token}");
        }

        return sub;
    }
    
    private static string ParseVerifiedEmailOrFail(JwtSecurityToken token)
    {
        var email = token.Claims.FirstOrDefault(x => x.Type == "email")?.Value;
        if (email is null)
        {
            throw new BadRequestException("Email not provided");
        }
        
        var emailVerified = token.Claims.FirstOrDefault(x => x.Type == "email_verified")?.Value;
        var parseResult = bool.TryParse(emailVerified, out var result);
        if (!parseResult)
        {
            throw new BadRequestException("Not provided email verified");
        }

        if (!result)
        {
            throw new BadRequestException("Email not verified");
        }

        return email;
    }
}
