using AnalyticsCustomers.Api.Data;
using AnalyticsCustomers.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsCustomers.Api.Services;

public class SubscriptionKeyService(AppDbContext db)
{
    public async Task<SubscriptionKey?> GetActiveKeyAsync(Guid userId) =>
        await db.SubscriptionKeys
            .Where(k => k.UserId == userId && k.IsActive)
            .OrderByDescending(k => k.IssuedAt)
            .FirstOrDefaultAsync();

    public async Task<List<SubscriptionKey>> GetHistoryAsync(Guid userId) =>
        await db.SubscriptionKeys
            .Where(k => k.UserId == userId)
            .OrderByDescending(k => k.IssuedAt)
            .ToListAsync();
}
