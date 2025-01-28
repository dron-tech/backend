using Application.Common.DTO.Users;
using Application.Common.Enums;
using MediatR;

namespace Application.Users.Commands.ThirdPartyLoginUser;

public class ThirdPartyLoginUserCmd : IRequest<SuccessAuthDto>
{
    public string AccessToken { get; set; } = string.Empty;
    public ThirdPartyPlatform? Platform { get; set; }
    public ThirdPartyServiceType Type { get; set; }
}
