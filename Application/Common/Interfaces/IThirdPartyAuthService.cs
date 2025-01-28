using Application.Common.DTO.Users;
using Application.Common.Enums;

namespace Application.Common.Interfaces;

public interface IThirdPartyAuthService
{
    public Task EnsureValidToken(string accessToken, ThirdPartyPlatform? platform = null);
    public Task<AuthProfileInfo> GetProfileInfo(string accessToken);
}
