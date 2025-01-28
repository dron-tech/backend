using MediatR;

namespace Application.Users.Commands.ResendConfirmEmail;

public class ResendConfirmEmailCmd : IRequest
{
    public int UserId { get; set; }
}
