using Domain.Common.Enums;

namespace Domain.Entities;

public class PublishOptions : BaseEntity
{
    public CommentType CommentType { get; set; }
    public LikeStyle LikeStyle { get; set; }
    public Nft? Nft { get; set; }
    public Video? Video { get; set; }
}
