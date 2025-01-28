using Application.Common.DTO.Users;
using Application.Profiles.Commands.UpdateProfile;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Attrs;
using WebApp.Common;
using WebApp.Common.DTO.Profiles;

namespace WebApp.Controllers;

[Authorize(Roles = UserRoles.User)]
[EnsureEmailIsConfirmAttr]
public class ProfilesController : BaseController
{
    public ProfilesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPut]
    public async Task<ApiResponse<UserDto>> UpdateProfile([FromForm] UpdateProfileDto dto)
    {
        var cmd = new UpdateProfileCmd
        {
            UserId = GetUserId(),
            Cover = dto.Cover,
            Avatar = dto.Avatar,
            Login = dto.Login,
            Name = dto.Name,
            Desc = dto.Desc,
            Status = dto.Status,
            Link = dto.Link
        };

        return GetFormattedResponse(await Mediator.Send(cmd));
    }
}
