using System.Collections.ObjectModel;

namespace Domain.Entities;

public class Video : BaseEntity
{
    public string FileNameFull { get; set; } = string.Empty;
    public string FileNameShort { get; set; } = string.Empty;
    public string? Desc { get; set; }
    public string? Location { get; set; }
    public string? Cover { get; set; }
    public TimeSpan Duration { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = default!;
    
    public PublishOptions PublishOptions { get; set; } = default!;
    public int PublishOptionsId { get; set; }

    public ICollection<Like> Likes { get; set; } = new Collection<Like>();
    public ICollection<Comment> Comments { get; set; } = new Collection<Comment>();
}
