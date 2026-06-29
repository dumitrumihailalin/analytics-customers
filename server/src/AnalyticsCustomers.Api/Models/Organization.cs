namespace AnalyticsCustomers.Api.Models;

public class Organization
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string? Country { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? Address { get; set; }
    public string? WebSite { get; set; }

    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Store> Stores { get; set; } = new List<Store>();
}
