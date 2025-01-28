using Application.Common.DTO.Users;
using MediatR;

namespace Application.Users.Commands.Login;

public class LoginCmd : IRequest<SuccessAuthDto>
{
    public string EmailOrLogin { get; set; } = string.Empty;
    public string Psw { get; set; } = string.Empty;
}
