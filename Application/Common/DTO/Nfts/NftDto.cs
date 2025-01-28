using Application.Common.DTO.PublishOptions;

namespace Application.Common.DTO.Nfts;

public class NftDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Desc { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? AnimationUrl { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsHide { get; set; }
    public string ExplorerUrl { get; set; } = string.Empty;
    
    public PublishOptionsDto? Options { get; set; }
    public int CommentCount { get; set; }
    public int LikeCount { get; set; }
    public bool IsLiked { get; set; }
}
