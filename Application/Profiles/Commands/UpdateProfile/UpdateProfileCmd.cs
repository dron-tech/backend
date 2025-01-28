using Application.Common.DTO.Users;
using Domain.Common.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Profiles.Commands.UpdateProfile;

public class UpdateProfileCmd : IRequest<UserDto>
{
    public int UserId { get; set; }
    public IFormFile? Cover { get; set; }
    public IFormFile? Avatar { get; set; }
    public string? Login { get; set; }

    public string? Name { get; set; }
    public string? Desc { get; set; }
    public UserStatus? Status { get; set; }

    public string? Link { get; set; }
}
