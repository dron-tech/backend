using MediatR;

namespace Application.Users.Commands.ResetPsw;

public class ResetPswCmd : IRequest
{
    public string EmailOrLogin { get; set; } = string.Empty;
    public int Code { get; set; }
    public string Psw { get; set; } = string.Empty;
}
