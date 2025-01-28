using System.Collections.ObjectModel;

namespace Domain.Entities;

public class User : BaseEntity
{
    public string Login { get; set; } = string.Empty;
    public string HashedPsw { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsEmailConfirm { get; set; }
    
    public string? AppleSub { get; set; }

    public DateTime LastLoginUpdate { get; set; }
    public int RoleId { get; set; }
    public Role Role { get; set; } = default!;
    
    public int? ConfirmCodeId { get; set; }
    public ConfirmCode? ConfirmCode { get; set; }

    public int? RefreshTokenId { get; set; }
    public RefreshToken? RefreshToken { get; set; }

    public int? ProfileId { get; set; }
    public Profile? Profile { get; set; }

    public ICollection<Video> Videos { get; set; } = new Collection<Video>();
    public ICollection<Nft> Nfts { get; set; } = new Collection<Nft>();
    
    public ICollection<Like> Likes { get; set; } = new Collection<Like>();
    public ICollection<Comment> Comments { get; set; } = new Collection<Comment>();

    public ICollection<Subscriber> Subscriptions { get; set; } = new Collection<Subscriber>();
}
