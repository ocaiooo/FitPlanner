namespace FitPlanner.Domain.Entities;

public class RefreshToken : EntityBase
{
    public string Token { get; set; } = string.Empty;
    public long UserId { get; set; }
    public User User { get; set; } = default!;
    public DateTime ExpiresOn { get; set; }
}