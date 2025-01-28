using Application.Common.DTO;
using Application.Common.DTO.Comments;
using Application.Common.Enums;
using Domain.Common.Enums;
using MediatR;

namespace Application.Comments.Queries.GetComments;

public class GetCommentsQuery : IRequest<PagedList<CommentDto>>
{
    public int EntityId { get; set; }
    public int UserId { get; set; }
    public ContentType Type { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}
