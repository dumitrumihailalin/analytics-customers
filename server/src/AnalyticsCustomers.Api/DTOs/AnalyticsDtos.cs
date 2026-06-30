namespace AnalyticsCustomers.Api.DTOs;

public record WeeklyRevenueDto(int Week, int Year, decimal TotalRevenue, int OrderCount, int QuantitySold);

public record MonthlyRevenueDto(int Month, int Year, decimal TotalRevenue, int OrderCount, int QuantitySold);

public record ProductBreakdownDto(string ProductId, decimal TotalRevenue, int RecordCount, int QuantitySold);

public record DashboardSummaryDto(
    decimal TotalRevenue,
    int TotalOrders,
    int TotalQuantitySold,
    decimal AverageOrderValue,
    List<WeeklyRevenueDto> WeeklyRevenue,
    List<MonthlyRevenueDto> MonthlyRevenue,
    List<ProductBreakdownDto> ProductBreakdown
);

public record SubscriptionKeyDto(
    Guid Id,
    string Key,
    DateTime IssuedAt,
    DateTime ExpiresAt,
    bool IsActive,
    int DaysRemaining,
    Guid? StoreId
);

public record GenerateKeyRequest(Guid? StoreId);

public record AssignStoreRequest(Guid StoreId);

public record AdminStatsDto(
    int TotalUsers,
    int ActiveSubscriptions,
    int TotalOrdersProcessed,
    decimal PlatformTotalRevenue
);
