using Application.Common.Enums;
using Application.Likes.Commands;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Attrs;

namespace WebApp.Controllers;

[Authorize(Roles = UserRoles.User)]
[EnsureEmailIsConfirmAttr]
public class LikesController : BaseController
{
    public LikesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("Nft/{nftId:int}")]
    public async Task LikeOrUnLikeNft(int nftId)
    {
        await LikeOrUnLike(nftId, GetUserId(), ContentType.Nft);
    }

    [HttpPost("Video/{videoId:int}")]
    public async Task LikeOrUnLikeVideo(int videoId)
    {
        await LikeOrUnLike(videoId, GetUserId(), ContentType.Video);
    }

    [HttpPost("Comment/{commentId:int}")]
    public async Task LikeOrUnLikeComment(int commentId)
    {
        await LikeOrUnLike(commentId, GetUserId(), ContentType.Comment);
    }

    private async Task LikeOrUnLike(int entityId, int userId, ContentType type)
    {
        var cmd = new LikeOrUnLikeCmd
        {
            UserId = userId,
            Id = entityId,
            Type = type
        };

        await Mediator.Send(cmd);
    }
}
