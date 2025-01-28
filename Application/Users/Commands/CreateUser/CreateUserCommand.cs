using Application.Common.DTO.Users;
using MediatR;

namespace Application.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<SuccessAuthDto>
{
    public string Login { get; set; } = string.Empty;
    public string Psw { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
