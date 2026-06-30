namespace AnalyticsCustomers.Api.DTOs;

public record OrderRequest(
    string OrderReference,
    decimal Amount,
    string Currency,
    string Status,
    string? ProductCategory,
    DateTime OrderDate
);

public record BulkOrderRequest(List<OrderRequest> Orders);

public record IngestRequest(
    string ProductId,
    decimal Price,
    int QuantitySold,
    int Stock
);

public record BulkIngestItem(
    Guid StoreId,
    string ProductId,
    decimal Price,
    int QuantitySold,
    int Stock
);

public record BulkIngestRequest(
    string ApiKey,
    List<BulkIngestItem> Items
);

public class AnalyticsIngestRequest
{
    public string ApiKey { get; set; } = null!;
    public Guid StoreId { get; set; }
    public string ProductId { get; set; } = null!;
    public decimal Price { get; set; }
    public int QuantitySold { get; set; }
    public int Stock { get; set; }
}

public record OrderResponse(
    Guid Id,
    string OrderReference,
    decimal Amount,
    string Currency,
    string Status,
    string? ProductCategory,
    DateTime OrderDate,
    int WeekNumber,
    int Year
);
