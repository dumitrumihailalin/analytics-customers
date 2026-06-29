namespace AnalyticsCustomers.Api.Models;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string OrderReference { get; set; } = string.Empty;   // product_id
    public decimal Price { get; set; }
    public int QuantitySold { get; set; }
    public int Stock { get; set; }
    public decimal Amount { get; set; }                          // Price * QuantitySold
    public string Currency { get; set; } = "USD";
    public string Status { get; set; } = string.Empty;
    public string? ProductCategory { get; set; }
    public DateTime OrderDate { get; set; }
    public int WeekNumber { get; set; }
    public int Year { get; set; }
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
}
