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
public class OrganizationsController(AppDbContext db) : ControllerBase
{
    // GET /api/organizations/mine
    // Any authenticated user — joins Organizations with Users on OrganizationId
    [HttpGet("mine")]
    public async Task<IActionResult> GetMine()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var org = await (
            from o in db.Organizations
            join u in db.Users on o.Id equals u.OrganizationId
            where u.Id == userId
            select new OrganizationResponse(
                o.Id, o.Name, o.Country, o.IsActive, o.CreatedAt,
                o.Address, o.WebSite,
                o.Users.Count, o.Stores.Count)
        ).FirstOrDefaultAsync();

        return org is null
            ? NotFound(new { error = "No organization assigned to your account." })
            : Ok(org);
    }

    // PUT /api/organizations/mine
    // Any authenticated user — updates their own organization
    [HttpPut("mine")]
    public async Task<IActionResult> UpdateMine([FromBody] UpdateOrganizationRequest req)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var org = await (
            from o in db.Organizations
            join u in db.Users on o.Id equals u.OrganizationId
            where u.Id == userId
            select o
        ).FirstOrDefaultAsync();

        if (org is null)
            return NotFound(new { error = "No organization assigned to your account." });

        org.Name = req.Name;
        org.Country = req.Country;
        org.Address = req.Address;
        org.WebSite = req.WebSite;
        await db.SaveChangesAsync();

        var userCount = await db.Users.CountAsync(u => u.OrganizationId == org.Id);
        var storeCount = await db.Organizations
            .Where(o => o.Id == org.Id)
            .Select(o => o.Stores.Count)
            .FirstOrDefaultAsync();

        return Ok(new OrganizationResponse(org.Id, org.Name, org.Country, org.IsActive, org.CreatedAt,
            org.Address, org.WebSite, userCount, storeCount));
    }

    // GET /api/organizations  (PlatformAdmin only)
    [HttpGet]
    [Authorize(Roles = "PlatformAdmin")]
    public async Task<IActionResult> GetAll()
    {
        var orgs = await db.Organizations
            .OrderBy(o => o.Name)
            .Select(o => new OrganizationResponse(
                o.Id, o.Name, o.Country, o.IsActive, o.CreatedAt,
                o.Address, o.WebSite,
                o.Users.Count, o.Stores.Count))
            .ToListAsync();

        return Ok(orgs);
    }

    // GET /api/organizations/{id}  (PlatformAdmin only)
    [HttpGet("{id:guid}")]
    [Authorize(Roles = "PlatformAdmin")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var org = await db.Organizations
            .Where(o => o.Id == id)
            .Select(o => new OrganizationResponse(
                o.Id, o.Name, o.Country, o.IsActive, o.CreatedAt,
                o.Address, o.WebSite,
                o.Users.Count, o.Stores.Count))
            .FirstOrDefaultAsync();

        return org is null ? NotFound() : Ok(org);
    }

    // POST /api/organizations  (PlatformAdmin only)
    [HttpPost]
    [Authorize(Roles = "PlatformAdmin")]
    public async Task<IActionResult> Create([FromBody] CreateOrganizationRequest req)
    {
        var org = new Organization
        {
            Name = req.Name,
            Country = req.Country,
            Address = req.Address,
            WebSite = req.WebSite
        };
        db.Organizations.Add(org);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = org.Id },
            new OrganizationResponse(org.Id, org.Name, org.Country, org.IsActive, org.CreatedAt,
                org.Address, org.WebSite, 0, 0));
    }

    // PUT /api/organizations/{id}  (PlatformAdmin only)
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "PlatformAdmin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOrganizationRequest req)
    {
        var org = await db.Organizations.FindAsync(id);
        if (org is null) return NotFound();

        org.Name = req.Name;
        org.Country = req.Country;
        org.Address = req.Address;
        org.WebSite = req.WebSite;
        await db.SaveChangesAsync();

        return Ok(new OrganizationResponse(org.Id, org.Name, org.Country, org.IsActive, org.CreatedAt,
            org.Address, org.WebSite, 0, 0));
    }

    // PATCH /api/organizations/{id}/toggle-active  (PlatformAdmin only)
    [HttpPatch("{id:guid}/toggle-active")]
    [Authorize(Roles = "PlatformAdmin")]
    public async Task<IActionResult> ToggleActive(Guid id)
    {
        var org = await db.Organizations.FindAsync(id);
        if (org is null) return NotFound();

        org.IsActive = !org.IsActive;
        await db.SaveChangesAsync();

        return Ok(new { org.Id, org.IsActive });
    }

    // DELETE /api/organizations/{id}  (PlatformAdmin only)
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "PlatformAdmin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var org = await db.Organizations
            .Include(o => o.Users)
            .Include(o => o.Stores)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (org is null) return NotFound();

        if (org.Users.Count > 0)
            return Conflict(new { error = $"Cannot delete: {org.Users.Count} user(s) still belong to this organization. Reassign them first." });

        db.Organizations.Remove(org);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
