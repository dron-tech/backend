namespace Domain.Entities;

public class ConfirmCode : BaseEntity
{
    public int Code { get; set; }
    public DateTime ExpiryAt { get; set; }

    public User User { get; set; } = default!;
}
