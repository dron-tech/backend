using Application.Common.DTO.Users;
using MediatR;

namespace Application.Users.Commands.ConfirmEmail;

public class ConfirmEmailCmd : IRequest<SuccessAuthDto>
{
    public int Code { get; set; }
    public int UserId { get; set; }
}
