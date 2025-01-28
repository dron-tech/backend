using Domain.Common.Enums;

namespace Application.Common.DTO.PublishOptions;

public class PublishOptionsDto
{
    public CommentType CommentType { get; set; }
    public LikeStyle LikeStyle { get; set; }
}
