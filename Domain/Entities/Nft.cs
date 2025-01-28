using System.Collections.ObjectModel;

namespace Domain.Entities;

public class Nft : BaseEntity
{
    public string? Desc { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? AnimationUrl { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ExplorerUrl { get; set; } = string.Empty;
    
    public int UserId { get; set; }
    public User User { get; set; } = default!;

    public PublishOptions PublishOptions { get; set; } = default!;
    public int PublishOptionsId { get; set; }

    public ICollection<Like> Likes { get; set; } = new Collection<Like>();
    public ICollection<Comment> Comments { get; set; } = new Collection<Comment>();
}
