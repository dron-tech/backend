using Application.Common.DTO.Users;
using MediatR;

namespace Application.Users.Queries.GetUserById;

public class GetUserByIdQuery : IRequest<UserDto>
{
    public int UserId { get; set; }
}
