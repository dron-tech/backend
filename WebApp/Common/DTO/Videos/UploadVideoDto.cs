using Domain.Common.Enums;

namespace WebApp.Common.DTO.Videos;

public class UploadVideoDto
{
    public IFormFile? Video { get; set; }
    public IFormFile? Cover { get; set; }
    public string? Desc { get; set; }
    public string? Location { get; set; }
    public CommentType CommentType { get; set; }
    public LikeStyle LikeStyle { get; set; }
}
