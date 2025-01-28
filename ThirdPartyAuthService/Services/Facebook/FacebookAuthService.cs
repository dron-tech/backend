using System.Net.Http.Json;
using System.Web;
using Application.Common.DTO.Users;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Microsoft.Extensions.Options;
using ThirdPartyAuthService.Services.Facebook.Common;
using ThirdPartyAuthService.Services.Facebook.Common.DTO;

namespace ThirdPartyAuthService.Services.Facebook;

public class FacebookAuthService : IThirdPartyAuthService
{
    private readonly HttpClient _httpClient;
    private readonly FacebookCfg _facebookCfg;

    private const string Url = "https://graph.facebook.com/";
    private const string GetAppTokenUrn = "oauth/access_token";
    private const string DebugTokenUrn = "debug_token";
    private const string GetMeUrn = "me";

    private const string AppTokenGrantType = "client_credentials";

    public FacebookAuthService(IOptions<FacebookCfg> options, HttpClient httpClient)
    {
        _facebookCfg = options.Value;
        _httpClient = httpClient;
    }

    public async Task EnsureValidToken(string accessToken, ThirdPartyPlatform? platform = null)
    {
        var appToken = await GetAppToken();
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["input_token"] = accessToken;
        query["access_token"] = appToken;

        var uri = new Uri($"{Url}{DebugTokenUrn}?{query}");
        var response = await _httpClient.GetAsync(uri);

        if (!response.IsSuccessStatusCode)
        {
            throw new BadRequestException("Incorrect facebook token");
        }

        var content = await response.Content.ReadFromJsonAsync<VerifyTokenResponse>();
        if (content is null || !content.Data.IsValid)
        {
            throw new BadRequestException("Incorrect facebook token");
        }

        EnsureValidScope(content.Data.Scope);
    }

    public async Task<AuthProfileInfo> GetProfileInfo(string accessToken)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["access_token"] = accessToken;
        query["fields"] = "id,name,email";
        
        var uri = new Uri($"{Url}{GetMeUrn}?{query}");
        var response = await _httpClient.GetAsync(uri);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<AuthProfileInfo>();
        if (content is null)
        {
            throw new Exception($"Response body from getting facebook profile info is null {response}");
        }

        return content;
    }
    
    private async Task<string> GetAppToken()
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["client_id"] = _facebookCfg.ClientId;
        query["client_secret"] = _facebookCfg.ClientSecret;
        query["grant_type"] = AppTokenGrantType;

        var uri = new Uri($"{Url}{GetAppTokenUrn}?{query}");
        var response = await _httpClient.GetAsync(uri);

        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadFromJsonAsync<AppTokenDto>();
        if (content?.AccessToken is null or "")
        {
            throw new Exception("Error near getting facebook app token - response body is invalid");
        }

        return content.AccessToken;
    }
    
    private void EnsureValidScope(string[] scope)
    {
        if (_facebookCfg.Scope.Any(item => !scope.Contains(item)))
        {
            throw new BadRequestException("Incorrect facebook token: invalid scope");
        }
    }
}
