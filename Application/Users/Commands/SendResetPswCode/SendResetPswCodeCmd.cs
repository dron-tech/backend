using MediatR;

namespace Application.Users.Commands.SendResetPswCode;

public class SendResetPswCodeCmd : IRequest
{
    public string EmailOrLogin { get; set; } = string.Empty;
}
