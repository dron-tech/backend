using Domain.Common.Enums;
using MediatR;

namespace Application.Nfts.Commands.AddNft;

public class AddNftCmd : IRequest<int>
{
    public int UserId { get; set; }
    public string? Desc { get; set; }
    public string Url { get; set; } = string.Empty;
    public CommentType CommentType { get; set; }
    public LikeStyle LikeStyle { get; set; }
}
