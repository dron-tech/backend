using Application.Common.Enums;

namespace Application.Common.Interfaces;

public interface IThirdPartyAuthFactory
{
    public IThirdPartyAuthService GetInstance(ThirdPartyServiceType type);
}
