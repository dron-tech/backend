using Application.Common.Enums;
using MediatR;

namespace Application.Likes.Commands;

public class LikeOrUnLikeCmd : IRequest
{
    public int UserId { get; set; }
    public int Id { get; set; }
    //public bool IsVideo { get; set; }
    public ContentType Type { get; set; }
}
