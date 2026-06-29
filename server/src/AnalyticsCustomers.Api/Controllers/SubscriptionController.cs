using System.Security.Claims;
using System.Security.Cryptography;
using AnalyticsCustomers.Api.Data;
using AnalyticsCustomers.Api.DTOs;
using AnalyticsCustomers.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsCustomers.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SubscriptionController(AppDbContext db) : ControllerBase
{
    [HttpGet("info")]
    public async Task<IActionResult> GetMySubscription()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await db.Users.FindAsync(userId);

        var activeKey = await db.SubscriptionKeys
            .Where(k => k.UserId == userId && k.IsActive)
            .OrderByDescending(k => k.IssuedAt)
            .Select(k => k.Key)
            .FirstOrDefaultAsync();

        if (user?.OrganizationId is null)
            return Ok(new
            {
                plan = "Free",
                isActive = false,
                startDate = DateTime.UtcNow,
                endDate = (DateTime?)null,
                key = activeKey,
                daysRemaining = 0
            });

        var sub = await db.Subscriptions
            .Where(s => s.OrganizationId == user.OrganizationId)
            .Join(db.Plans, s => s.PlanId, p => p.Id, (s, p) => new { s, planName = p.Name })
            .FirstOrDefaultAsync();

        if (sub is null)
            return Ok(new
            {
                plan = "Free",
                isActive = true,
                startDate = DateTime.UtcNow,
                endDate = (DateTime?)null,
                key = activeKey,
                daysRemaining = 0
            });

        var daysRemaining = sub.s.EndDate.HasValue
            ? Math.Max(0, (int)(sub.s.EndDate.Value - DateTime.UtcNow).TotalDays)
            : 365;

        return Ok(new
        {
            plan = sub.planName,
            sub.s.IsActive,
            sub.s.StartDate,
            sub.s.EndDate,
            key = activeKey,
            daysRemaining
        });
    }

    [HttpPost("key/generate")]
    public async Task<IActionResult> GenerateKey()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await db.Users.FindAsync(userId);

        var existing = await db.SubscriptionKeys
            .Where(k => k.UserId == userId && k.IsActive)
            .ToListAsync();
        foreach (var old in existing) old.IsActive = false;

        var rawBytes = RandomNumberGenerator.GetBytes(24);
        var rawKey = "sk_" + Convert.ToBase64String(rawBytes)
            .Replace("+", "a").Replace("/", "b").Replace("=", "")[..32];

        var key = new SubscriptionKey
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Key = rawKey,
            IssuedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddYears(1),
            IsActive = true
        };
        db.SubscriptionKeys.Add(key);
        await db.SaveChangesAsync();

        return Ok(new {
            key.Id,
            key.Key,
            key.IssuedAt,
            key.ExpiresAt,
            key.IsActive,
            OrganizationId = user?.OrganizationId
        });
    }

    [HttpGet("key/history")]
    public async Task<ActionResult<List<SubscriptionKeyDto>>> GetKeyHistory()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var keys = await db.SubscriptionKeys
            .Where(k => k.UserId == userId)
            .OrderByDescending(k => k.IssuedAt)
            .ToListAsync();

        return Ok(keys.Select(k => new SubscriptionKeyDto(
            k.Id, k.Key, k.IssuedAt, k.ExpiresAt, k.IsActive,
            Math.Max(0, (int)(k.ExpiresAt - DateTime.UtcNow).TotalDays)
        )));
    }
}
