namespace Domain.Entities;

public class Like : BaseEntity
{
    public int? NftId { get; set; }
    public Nft? Nft { get; set; }
    
    public int? VideoId { get; set; }
    public Video? Video { get; set; }
    
    public int? CommentId { get; set; }
    public Comment? Comment { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; } = default!;
}
