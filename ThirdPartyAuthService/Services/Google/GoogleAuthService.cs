using System.Net.Http.Json;
using Application.Common.DTO.Users;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Microsoft.Extensions.Options;
using ThirdPartyAuthService.Services.Google.Common;
using ThirdPartyAuthService.Services.Google.Common.DTO;

namespace ThirdPartyAuthService.Services.Google;

public class GoogleAuthService : IThirdPartyAuthService
{
    private readonly HttpClient _httpClient;
    private readonly GoogleCfg _googleCfg;
    
    private const string Url = "https://www.googleapis.com/oauth2/v1/";
    private const string GetTokenInfoUrn = "tokeninfo";
    private const string GetProfileUrn = "userinfo";

    private const string AuthHeaderName = "Authorization";
    private const string AuthTokenPrefix = "Bearer";

    public GoogleAuthService(HttpClient httpClient, IOptions<GoogleCfg> options)
    {
        _httpClient = httpClient;
        _googleCfg = options.Value;
    }

    public async Task EnsureValidToken(string accessToken, ThirdPartyPlatform? platform)
    {
        var query = $"access_token={accessToken}";
        var uri = new Uri($"{Url}{GetTokenInfoUrn}?{query}");

        var response = await _httpClient.GetAsync(uri);
        if (!response.IsSuccessStatusCode)
        {
            throw new BadRequestException("Incorrect google token");
        }

        var content = await response.Content.ReadFromJsonAsync<TokenInfoDto>();
        if (content is null)
        {
            throw new BadRequestException("Incorrect google token");
        } 
        
        EnsureValidScope(content.Scope);
        if (!content.VerifiedEmail)
        {
            throw new BadRequestException("Incorrect google token: email not verify");
        }

        var client = platform switch
        {
            ThirdPartyPlatform.Android => _googleCfg.Clients
                .First(x => x.Platform is ThirdPartyPlatform.Android),
            ThirdPartyPlatform.Ios => _googleCfg.Clients
                .First(x => x.Platform is ThirdPartyPlatform.Ios),
            null => throw new Exception("For google auth need provide platform type"),
            _ => throw new ArgumentOutOfRangeException(nameof(platform), "platform", "Platform out of range")
        };
        
        if (content.Audience != client.ClientId || content.IssuedTo != client.ClientId)
        {
            throw new BadRequestException("Incorrect google token: aud or iss incorrect");
        }
    }

    public async Task<AuthProfileInfo> GetProfileInfo(string accessToken)
    {
        var uri = new Uri($"{Url}{GetProfileUrn}");

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = uri
        };
        
        request.Headers.Add(AuthHeaderName, $"{AuthTokenPrefix} {accessToken}");
        
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<AuthProfileInfo>();
        if (content is null)
        {
            throw new Exception($"Response body from getting google profile info is null {response}");
        }

        return content;
    }

    private void EnsureValidScope(string scope)
    {
        var splitScope = scope.Split(" ");
        if (_googleCfg.Scope.Any(item => !splitScope.Contains(item)))
        {
            throw new BadRequestException("Incorrect google token: invalid scope");
        }
    }
}
