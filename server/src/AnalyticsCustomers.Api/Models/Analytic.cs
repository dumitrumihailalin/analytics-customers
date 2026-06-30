namespace AnalyticsCustomers.Api.Models;

public class Analytic
{
    public Guid Id { get; set; }
    public Guid StoreId { get; set; }
    public string ProductId { get; set; } = null!;
    public decimal Price { get; set; }
    public int QuantitySold { get; set; }
    public int Stock { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Store Store { get; set; } = null!;
}
