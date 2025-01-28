using System.Collections.ObjectModel;

namespace Domain.Entities;

public class Comment : BaseEntity
{
    public string Value { get; set; } = string.Empty;
    
    public int? NftId { get; set; }
    public Nft? Nft { get; set; }
    
    public int? VideoId { get; set; }
    public Video? Video { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; } = default!;
    
    public int? ReplyId { get; set; }
    public Comment? Reply { get; set; }

    public ICollection<Like> Likes { get; set; } = new Collection<Like>();
    public ICollection<Comment> Replies { get; set; } = new Collection<Comment>();
}
