namespace Domain.Entities;

public class Subscriber : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = default!;
    
    public int FollowsId { get; set; }
}
