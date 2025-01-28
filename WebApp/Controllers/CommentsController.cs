using Application.Comments.Commands.AddComment;
using Application.Comments.Commands.AddReply;
using Application.Comments.Queries.GetComments;
using Application.Common.DTO;
using Application.Common.DTO.Comments;
using Application.Common.Enums;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Attrs;
using WebApp.Common;
using WebApp.Common.DTO;
using WebApp.Common.DTO.Comments;

namespace WebApp.Controllers;

[Authorize(Roles = UserRoles.User)]
[EnsureEmailIsConfirmAttr]
public class CommentsController : BaseController
{
    public CommentsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("{entityId:int}")]
    public async Task<ApiResponse<int>> AddComment(AddCommentDto dto, int entityId)
    {
        var cmd = new AddCommentCmd
        {
            UserId = GetUserId(),
            EntityId = entityId,
            IsVideo = dto.IsVideo,
            Value = dto.Value
        };

        return GetFormattedResponse(await Mediator.Send(cmd));
    }

    [HttpPost("{commentId:int}/reply")]
    public async Task<ApiResponse<int>> AddReply(AddReplyDto dto, int commentId)
    {
        var cmd = new AddReplyCmd
        {
            UserId = GetUserId(),
            CommentId = commentId,
            Value = dto.Value
        };

        return GetFormattedResponse(await Mediator.Send(cmd));
    }

    [HttpGet("Nft/{nftId:int}")]
    public async Task<ApiResponse<PagedList<CommentDto>>> GetNftCommentList(int nftId, [FromQuery] GetPagedListDto dto)
    {
        return GetFormattedResponse(await GetComments(nftId, GetUserId(), ContentType.Nft, dto));
    }
    
    [HttpGet("Video/{videoId:int}")]
    public async Task<ApiResponse<PagedList<CommentDto>>> GetVideoCommentList(int videoId, [FromQuery] GetPagedListDto dto)
    {
        return GetFormattedResponse(await GetComments(videoId, GetUserId(), ContentType.Video, dto));
    }
    
    [HttpGet("Comment/{commentId:int}")]
    public async Task<ApiResponse<PagedList<CommentDto>>> GetReplyCommentList(int commentId, [FromQuery] GetPagedListDto dto)
    {
        return GetFormattedResponse(await GetComments(commentId, GetUserId(), ContentType.Comment, dto));
    }

    private Task<PagedList<CommentDto>> GetComments(int entityId, int userId, ContentType type, GetPagedListDto dto)
    {
        var cmd = new GetCommentsQuery
        {
            EntityId = entityId,
            UserId = userId,
            Type = type,
            PageIndex = dto.PageIndex,
            PageSize = dto.PageSize
        };

        return Mediator.Send(cmd);
    }
}
