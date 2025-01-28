using Application.Common.DTO.PublishOptions;

namespace Application.Common.DTO.Videos;

public class VideoDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public string FileNameFull { get; set; } = string.Empty;
    public string FileNameShort { get; set; } = string.Empty;
    public string? Desc { get; set; }
    public string? Location { get; set; }
    public string? Cover { get; set; }
    public bool IsLiked { get; set; }
    public TimeSpan Duration { get; set; }
    public PublishOptionsDto? Options { get; set; } 
    public int CommentCount { get; set; }
    public int LikeCount { get; set; }
}
