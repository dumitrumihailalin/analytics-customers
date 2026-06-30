using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
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
public class ApiKeysController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetByStore([FromQuery] Guid storeId)
    {
        var keys = await (
            from k in db.ApiKeys
            join s in db.Stores on k.StoreId equals s.Id
            where k.StoreId == storeId
            orderby k.CreatedAt descending
            select new ApiKeyResponse(
                k.Id, k.StoreId, s.StoreName, k.Prefix,
                k.IsActive, k.CreatedAt, k.ExpiresAt, k.LastUsedAt)
        ).ToListAsync();

        return Ok(keys);
    }

    [HttpPost]
    [Authorize(Roles = "PlatformAdmin")]
    public async Task<IActionResult> Generate([FromBody] CreateApiKeyRequest req)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var level = User.FindFirstValue(ClaimTypes.Role) ?? "User";

        var store = await db.Stores.Include(s => s.Organization).FirstOrDefaultAsync(s => s.Id == req.StoreId);
        if (store is null) return BadRequest(new { error = "Store not found." });

        if (level != "PlatformAdmin")
        {
            var user = await db.Users.FindAsync(userId);
            if (user?.OrganizationId != store.OrganizationId) return Forbid();
        }

        var rawBytes = RandomNumberGenerator.GetBytes(32);
        var rawKey = "ak_" + Convert.ToBase64String(rawBytes).Replace("+", "").Replace("/", "").Replace("=", "")[..40];
        var prefix = rawKey[..8];
        var keyHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(rawKey))).ToLower();

        var apiKey = new ApiKey
        {
            StoreId = req.StoreId,
            KeyHash = keyHash,
            Prefix = prefix,
            ExpiresAt = req.ExpiresAt,
            IsActive = true
        };
        db.ApiKeys.Add(apiKey);
        await db.SaveChangesAsync();

        return Ok(new GeneratedApiKeyResponse(
            apiKey.Id, apiKey.StoreId, store.StoreName, apiKey.Prefix,
            rawKey, apiKey.IsActive, apiKey.CreatedAt, apiKey.ExpiresAt));
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var level = User.FindFirstValue(ClaimTypes.Role) ?? "User";

        var key = await db.ApiKeys.Include(k => k.Store).FirstOrDefaultAsync(k => k.Id == id);
        if (key is null) return NotFound();

        if (level != "PlatformAdmin")
        {
            var user = await db.Users.FindAsync(userId);
            if (user?.OrganizationId != key.Store.OrganizationId) return Forbid();
        }

        key.IsActive = false;
        await db.SaveChangesAsync();

        return Ok(new { key.Id, key.IsActive });
    }
}
