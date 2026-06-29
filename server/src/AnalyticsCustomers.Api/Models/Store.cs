namespace AnalyticsCustomers.Api.Models;

public class Store
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid OrganizationId { get; set; }
    public string StoreName { get; set; } = null!;
    public string? Street { get; set; }
    public string? Country { get; set; }
    public string? Website { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Organization Organization { get; set; } = null!;
    public ICollection<Analytic> Analytics { get; set; } = new List<Analytic>();
    public ICollection<ApiKey> ApiKeys { get; set; } = new List<ApiKey>();
}
