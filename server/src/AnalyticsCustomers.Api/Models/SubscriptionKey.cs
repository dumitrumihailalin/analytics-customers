using System.ComponentModel.DataAnnotations.Schema;

namespace AnalyticsCustomers.Api.Models;

public class SubscriptionKey
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Key { get; set; } = string.Empty;
    public DateTime IssuedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsActive { get; set; }
    public Guid? StoreId { get; set; }

    [NotMapped] public User? User { get; set; }
}
