using AnalyticsCustomers.Api.Data;
using AnalyticsCustomers.Api.DTOs;
using AnalyticsCustomers.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsCustomers.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngestController(AppDbContext db) : ControllerBase
{
    /// POST /api/ingest/analytics
    [HttpPost("analytics")]
    public async Task<IActionResult> IngestAnalytic([FromBody] AnalyticsIngestRequest req)
    {
        var key = await db.SubscriptionKeys
            .FirstOrDefaultAsync(k => k.Key == req.ApiKey && k.IsActive);

        if (key is null)
            return Unauthorized(new { error = "Invalid or inactive API key." });

        if (key.ExpiresAt < DateTime.UtcNow)
            return StatusCode(403, new { error = "API key has expired." });

        var record = new Analytic
        {
            StoreId = req.StoreId,
            ProductId = req.ProductId,
            Price = req.Price,
            QuantitySold = req.QuantitySold,
            Stock = req.Stock
        };

        db.Analytics.Add(record);
        await db.SaveChangesAsync();

        return Ok(new { record.Id, record.StoreId, record.CreatedAt });
    }

    /// POST /api/ingest/analytics/bulk
    [HttpPost("analytics/bulk")]
    public async Task<IActionResult> IngestAnalyticsBulk([FromBody] BulkIngestRequest req)
    {
        var key = await db.SubscriptionKeys
            .FirstOrDefaultAsync(k => k.Key == req.ApiKey && k.IsActive);

        if (key is null)
            return Unauthorized(new { error = "Invalid or inactive API key." });

        if (key.ExpiresAt < DateTime.UtcNow)
            return StatusCode(403, new { error = "API key has expired." });

        if (req.Items is null || req.Items.Count == 0)
            return BadRequest(new { error = "Items list cannot be empty." });

        var now = DateTime.UtcNow;
        var records = req.Items.Select(item => new Analytic
        {
            StoreId = item.StoreId,
            ProductId = item.ProductId,
            Price = item.Price,
            QuantitySold = item.QuantitySold,
            Stock = item.Stock,
            CreatedAt = now
        }).ToList();

        db.Analytics.AddRange(records);
        await db.SaveChangesAsync();

        return Ok(new
        {
            Accepted = records.Count,
            CreatedAt = now,
            Ids = records.Select(r => r.Id)
        });
    }
}
