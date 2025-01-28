using Application.Common.Enums;
using Application.Common.Interfaces;
using ThirdPartyAuthService.Services.Facebook;
using ThirdPartyAuthService.Services.Google;

namespace ThirdPartyAuthService;

public class ThirdPartyAuthFactory : IThirdPartyAuthFactory
{
    private readonly IEnumerable<IThirdPartyAuthService> _authServices;

    public ThirdPartyAuthFactory(IEnumerable<IThirdPartyAuthService> authServices)
    {
        _authServices = authServices;
    }

    public IThirdPartyAuthService GetInstance(ThirdPartyServiceType type)
    {
        return type switch
        {
            ThirdPartyServiceType.Facebook => GetService(typeof(FacebookAuthService)),
            ThirdPartyServiceType.Google => GetService(typeof(GoogleAuthService)),
            _ => throw new InvalidOperationException("Not valid service type for auth service")
        };
    }

    private IThirdPartyAuthService GetService(Type type)
    {
        return _authServices.First(x => x.GetType() == type);
    }
}
