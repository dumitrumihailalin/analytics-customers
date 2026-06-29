using AnalyticsCustomers.Api.DTOs;

namespace AnalyticsCustomers.Api.Services;

public class OrderService
{
    public Task<List<OrderResponse>> SubmitBulkAsync(Guid userId, BulkOrderRequest request)
        => Task.FromResult(new List<OrderResponse>());
}
