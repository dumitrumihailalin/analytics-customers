using System.Globalization;
using AnalyticsCustomers.Api.Data;
using AnalyticsCustomers.Api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsCustomers.Api.Services;

public class AnalyticsService(AppDbContext db)
{
    public async Task<DashboardSummaryDto> GetDashboardAsync(Guid userId, int? year = null)
    {
        var targetYear = year ?? DateTime.UtcNow.Year;

        var user = await db.Users.FindAsync(userId);
        if (user?.OrganizationId is null)
            return new DashboardSummaryDto(0, 0, 0, 0, [], [], []);

        var storeIds = await db.Stores
            .Where(s => s.OrganizationId == user.OrganizationId)
            .Select(s => s.Id)
            .ToListAsync();

        if (storeIds.Count == 0)
            return new DashboardSummaryDto(0, 0, 0, 0, [], [], []);

        var records = await db.Analytics
            .Where(a => storeIds.Contains(a.StoreId) && a.RecordedAt.Year == targetYear)
            .ToListAsync();

        var weekly = records
            .GroupBy(a => new { Week = ISOWeek.GetWeekOfYear(a.RecordedAt), a.RecordedAt.Year })
            .Select(g => new WeeklyRevenueDto(
                g.Key.Week, g.Key.Year,
                g.Sum(a => a.Price * a.QuantitySold),
                g.Count(),
                g.Sum(a => a.QuantitySold)))
            .OrderBy(w => w.Week)
            .ToList();

        var monthly = records
            .GroupBy(a => new { a.RecordedAt.Month, a.RecordedAt.Year })
            .Select(g => new MonthlyRevenueDto(
                g.Key.Month, g.Key.Year,
                g.Sum(a => a.Price * a.QuantitySold),
                g.Count(),
                g.Sum(a => a.QuantitySold)))
            .OrderBy(m => m.Month)
            .ToList();

        var categories = records
            .GroupBy(a => a.CategoryId)
            .Select(g => new CategoryBreakdownDto(
                g.Key,
                g.Sum(a => a.Price * a.QuantitySold),
                g.Count(),
                g.Sum(a => a.QuantitySold)))
            .OrderByDescending(c => c.TotalRevenue)
            .ToList();

        var totalRevenue = records.Sum(a => a.Price * a.QuantitySold);
        var totalQty = records.Sum(a => a.QuantitySold);
        var count = records.Count;

        return new DashboardSummaryDto(
            totalRevenue,
            count,
            totalQty,
            count > 0 ? Math.Round(totalRevenue / count, 2) : 0,
            weekly,
            monthly,
            categories);
    }

    public async Task<AdminStatsDto> GetAdminStatsAsync()
    {
        var totalUsers = await db.Users.CountAsync();
        var activeSubs = await db.Subscriptions.CountAsync(s => s.IsActive);

        return new AdminStatsDto(totalUsers, activeSubs, 0, 0);
    }
}
