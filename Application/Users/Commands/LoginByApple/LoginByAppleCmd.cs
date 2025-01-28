using Application.Common.DTO.Users;
using MediatR;

namespace Application.Users.Commands.LoginByApple;

public class LoginByAppleCmd : IRequest<SuccessAuthDto>
{
    public string Code { get; set; } = string.Empty;
}
