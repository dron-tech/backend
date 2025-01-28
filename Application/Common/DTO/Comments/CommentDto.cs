using Application.Common.DTO.Profiles;

namespace Application.Common.DTO.Comments;

public class CommentDto
{
    public int Id { get; set; }
    public string Value { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string Login { get; set; } = string.Empty;
    public ProfileDto Profile { get; set; } = new();
    public int LikeCount { get; set; }
    public bool IsLiked { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CommentCount { get; set; }
}
