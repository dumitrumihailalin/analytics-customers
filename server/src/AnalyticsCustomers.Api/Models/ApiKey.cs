namespace AnalyticsCustomers.Api.Models;

public class ApiKey
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid StoreId { get; set; }
    public string KeyHash { get; set; } = null!;
    public string Prefix { get; set; } = null!;
    public DateTime? ExpiresAt { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastUsedAt { get; set; }

    public Store Store { get; set; } = null!;
}
