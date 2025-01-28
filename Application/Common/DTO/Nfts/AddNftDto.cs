using Domain.Common.Enums;

namespace Application.Common.DTO.Nfts;

public class AddNftDto
{
    public string? Desc { get; set; }
    public string Url { get; set; } = string.Empty;
    public CommentType CommentType { get; set; }
    public LikeStyle LikeStyle { get; set; }
}
