namespace Domain.Entities;

public class RefreshToken : BaseEntity
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiryAt { get; set; }
    
    public User User { get; set; } = default!;
}
