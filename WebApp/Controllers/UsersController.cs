using Application.Common.DTO.Users;
using Application.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Common;

namespace WebApp.Controllers;

[Authorize]
public class UsersController : BaseController
{
    public UsersController(IMediator mediator) : base(mediator)
    {
    }
    
    [HttpGet]
    public async Task<ApiResponse<UserDto>> GetMe()
    {
        var query = new GetUserByIdQuery
        {
            UserId = GetUserId()
        };

        return GetFormattedResponse(await Mediator.Send(query));
    }
}
