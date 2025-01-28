using Domain.Common.Enums;

namespace WebApp.Common.DTO.Nfts;

public class UpdateNftDto
{
    public string? Desc { get; set; }
    public CommentType? CommentType { get; set; }
    public LikeStyle? LikeStyle { get; set; }
}
