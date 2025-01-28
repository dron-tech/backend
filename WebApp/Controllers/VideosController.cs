using Application.Common.DTO;
using Application.Common.DTO.Videos;
using Application.Videos.Commands.DeleteVideo;
using Application.Videos.Commands.UploadVideo;
using Application.Videos.Queries.GetUserVideoList;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Attrs;
using WebApp.Common;
using WebApp.Common.DTO;
using WebApp.Common.DTO.Videos;

namespace WebApp.Controllers;

[Authorize(Roles = UserRoles.User)]
[EnsureEmailIsConfirmAttr]
public class VideosController : BaseController
{
    private const int MaxRequestSizeOnUpload = 310_000_000; 
    public VideosController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    [RequestSizeLimit(MaxRequestSizeOnUpload)]
    public async Task<ApiResponse<int>> UploadVideo([FromForm] UploadVideoDto dto)
    {
        var cmd = new UploadVideoCmd
        {
            UserId = GetUserId(),
            Video = dto.Video,
            Cover = dto.Cover,
            Desc = dto.Desc,
            Location = dto.Location,
            CommentType = dto.CommentType,
            LikeStyle = dto.LikeStyle
        };

        Response.StatusCode = 201;
        return GetFormattedResponse(await Mediator.Send(cmd), 201);
    }

    [HttpDelete("{videoId:int}")]
    public async Task DeleteVideo(int videoId)
    {
        var cmd = new DeleteVideoCmd
        {
            UserId = GetUserId(),
            VideoId = videoId
        };

        await Mediator.Send(cmd);
    }

    [HttpGet("All")]
    public async Task<ApiResponse<PagedList<VideoDto>>> GetMy([FromQuery] GetPagedListDto dto)
    {
        var qry = new GetUserVideoListQuery
        {
            UserId = GetUserId(),
            PageIndex = dto.PageIndex,
            PageSize = dto.PageSize
        };

        return GetFormattedResponse(await Mediator.Send(qry));
    }

}
