using Application.Common.DTO;
using Application.Common.DTO.Videos;
using MediatR;

namespace Application.Videos.Queries.GetUserVideoList;

public class GetUserVideoListQuery : IRequest<PagedList<VideoDto>>
{
    public int UserId { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}
