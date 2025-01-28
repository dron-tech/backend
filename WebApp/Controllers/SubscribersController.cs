using Application.Subscribers.Commands.SubscribeOrUnSubscribe;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Attrs;

namespace WebApp.Controllers;

[Authorize(Roles = UserRoles.User)]
[EnsureEmailIsConfirmAttr]
public class SubscribersController : BaseController
{
    public SubscribersController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("{publisherId:int}")]
    public async Task SubscribeOrUnSubscribe(int publisherId)
    {
        var cmd = new SubOrUnSubCmd
        {
            UserId = GetUserId(),
            PublisherId = publisherId
        };

        await Mediator.Send(cmd);
    }
}
