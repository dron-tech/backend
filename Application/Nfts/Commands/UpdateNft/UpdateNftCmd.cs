using Domain.Common.Enums;
using MediatR;

namespace Application.Nfts.Commands.UpdateNft;

public class UpdateNftCmd : IRequest
{
    public int NftId { get; set; }
    public int UserId { get; set; }
    public string? Desc { get; set; }
    public CommentType? CommentType { get; set; }
    public LikeStyle? LikeStyle { get; set; }
}
