using AnalyticsCustomers.Api.Data;
using AnalyticsCustomers.Api.DTOs;
using AnalyticsCustomers.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsCustomers.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "PlatformAdmin")]
public class OrganizationsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orgs = await db.Organizations
            .Select(o => new OrganizationResponse(
                o.Id, o.Name, o.Country, o.IsActive, o.CreatedAt,
                o.Users.Count, o.Stores.Count))
            .OrderBy(o => o.Name)
            .ToListAsync();

        return Ok(orgs);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var org = await db.Organizations
            .Where(o => o.Id == id)
            .Select(o => new OrganizationResponse(
                o.Id, o.Name, o.Country, o.IsActive, o.CreatedAt,
                o.Users.Count, o.Stores.Count))
            .FirstOrDefaultAsync();

        return org is null ? NotFound() : Ok(org);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrganizationRequest req)
    {
        var org = new Organization
        {
            Name = req.Name,
            Country = req.Country
        };
        db.Organizations.Add(org);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = org.Id },
            new OrganizationResponse(org.Id, org.Name, org.Country, org.IsActive, org.CreatedAt, 0, 0));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOrganizationRequest req)
    {
        var org = await db.Organizations.FindAsync(id);
        if (org is null) return NotFound();

        org.Name = req.Name;
        org.Country = req.Country;
        await db.SaveChangesAsync();

        return Ok(new OrganizationResponse(org.Id, org.Name, org.Country, org.IsActive, org.CreatedAt, 0, 0));
    }

    [HttpPatch("{id:guid}/toggle-active")]
    public async Task<IActionResult> ToggleActive(Guid id)
    {
        var org = await db.Organizations.FindAsync(id);
        if (org is null) return NotFound();

        org.IsActive = !org.IsActive;
        await db.SaveChangesAsync();

        return Ok(new { org.Id, org.IsActive });
    }

    [HttpDelete("{id:guid}")]
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
