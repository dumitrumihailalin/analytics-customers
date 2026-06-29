namespace AnalyticsCustomers.Api.DTOs;

public record WeeklyRevenueDto(int Week, int Year, decimal TotalRevenue, int OrderCount, int QuantitySold);

public record MonthlyRevenueDto(int Month, int Year, decimal TotalRevenue, int OrderCount, int QuantitySold);

public record CategoryBreakdownDto(string Category, decimal TotalRevenue, int OrderCount, int QuantitySold);

public record DashboardSummaryDto(
    decimal TotalRevenue,
    int TotalOrders,
    int TotalQuantitySold,
    decimal AverageOrderValue,
    List<WeeklyRevenueDto> WeeklyRevenue,
    List<MonthlyRevenueDto> MonthlyRevenue,
    List<CategoryBreakdownDto> CategoryBreakdown
);

public record SubscriptionKeyDto(
    Guid Id,
    string Key,
    DateTime IssuedAt,
    DateTime ExpiresAt,
    bool IsActive,
    int DaysRemaining
);

public record AdminStatsDto(
    int TotalUsers,
    int ActiveSubscriptions,
    int TotalOrdersProcessed,
    decimal PlatformTotalRevenue
);
