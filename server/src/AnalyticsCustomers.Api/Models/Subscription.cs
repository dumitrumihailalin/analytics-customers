namespace AnalyticsCustomers.Api.Models;

public class Subscription
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid OrganizationId { get; set; }
    public Guid PlanId { get; set; }
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; } = true;

    public Organization Organization { get; set; } = null!;
}
