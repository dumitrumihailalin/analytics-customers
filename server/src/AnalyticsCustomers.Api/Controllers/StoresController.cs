using System.Security.Claims;
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
public class StoresController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? organizationId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var level = User.FindFirstValue(ClaimTypes.Role) ?? "User";

        IQueryable<Store> query = db.Stores.Include(s => s.Organization).Include(s => s.ApiKeys);

        if (level == "PlatformAdmin")
        {
            if (organizationId.HasValue)
                query = query.Where(s => s.OrganizationId == organizationId.Value);
        }
        else
        {
            var user = await db.Users.FindAsync(userId);
            if (user?.OrganizationId is null) return Ok(Array.Empty<StoreResponse>());
            query = query.Where(s => s.OrganizationId == user.OrganizationId);
        }

        var stores = await query
            .OrderBy(s => s.StoreName)
            .Select(s => new StoreResponse(
                s.Id, s.OrganizationId, s.Organization.Name,
                s.StoreName, s.Street, s.Country, s.Website,
                s.CreatedAt, s.ApiKeys.Count))
            .ToListAsync();

        return Ok(stores);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var level = User.FindFirstValue(ClaimTypes.Role) ?? "User";

        var store = await db.Stores
            .Include(s => s.Organization)
            .Include(s => s.ApiKeys)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (store is null) return NotFound();

        if (level != "PlatformAdmin")
        {
            var user = await db.Users.FindAsync(userId);
            if (user?.OrganizationId != store.OrganizationId) return Forbid();
        }

        return Ok(new StoreResponse(
            store.Id, store.OrganizationId, store.Organization.Name,
            store.StoreName, store.Street, store.Country, store.Website,
            store.CreatedAt, store.ApiKeys.Count));
    }

    [HttpPost]
    [Authorize(Roles = "PlatformAdmin,OrgAdmin")]
    public async Task<IActionResult> Create([FromBody] CreateStoreRequest req)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var level = User.FindFirstValue(ClaimTypes.Role) ?? "User";

        if (level != "PlatformAdmin")
        {
            var user = await db.Users.FindAsync(userId);
            if (user?.OrganizationId != req.OrganizationId) return Forbid();
        }

        var org = await db.Organizations.FindAsync(req.OrganizationId);
        if (org is null) return BadRequest(new { error = "Organization not found." });

        var store = new Store
        {
            OrganizationId = req.OrganizationId,
            StoreName = req.StoreName,
            Street = req.Street,
            Country = req.Country,
            Website = req.Website
        };
        db.Stores.Add(store);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = store.Id },
            new StoreResponse(store.Id, store.OrganizationId, org.Name,
                store.StoreName, store.Street, store.Country, store.Website, store.CreatedAt, 0));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "PlatformAdmin,OrgAdmin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateStoreRequest req)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var level = User.FindFirstValue(ClaimTypes.Role) ?? "User";

        var store = await db.Stores.Include(s => s.Organization).FirstOrDefaultAsync(s => s.Id == id);
        if (store is null) return NotFound();

        if (level != "PlatformAdmin")
        {
            var user = await db.Users.FindAsync(userId);
            if (user?.OrganizationId != store.OrganizationId) return Forbid();
        }

        store.StoreName = req.StoreName;
        store.Street = req.Street;
        store.Country = req.Country;
        store.Website = req.Website;
        await db.SaveChangesAsync();

        return Ok(new StoreResponse(
            store.Id, store.OrganizationId, store.Organization.Name,
            store.StoreName, store.Street, store.Country, store.Website, store.CreatedAt, 0));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "PlatformAdmin,OrgAdmin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var level = User.FindFirstValue(ClaimTypes.Role) ?? "User";

        var store = await db.Stores.FindAsync(id);
        if (store is null) return NotFound();

        if (level != "PlatformAdmin")
        {
            var user = await db.Users.FindAsync(userId);
            if (user?.OrganizationId != store.OrganizationId) return Forbid();
        }

        db.Stores.Remove(store);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
