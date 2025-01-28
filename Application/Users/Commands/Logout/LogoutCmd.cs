using MediatR;

namespace Application.Users.Commands.Logout;

public class LogoutCmd : IRequest
{
    public int UserId { get; set; }
}
