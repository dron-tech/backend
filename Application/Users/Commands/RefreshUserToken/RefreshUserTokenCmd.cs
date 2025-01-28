using Application.Common.DTO.Users;
using MediatR;

namespace Application.Users.Commands.RefreshUserToken;

public class RefreshUserTokenCmd : IRequest<SuccessAuthDto>
{
    public int UserId { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
}
