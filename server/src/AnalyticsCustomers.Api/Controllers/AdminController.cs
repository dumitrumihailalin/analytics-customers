using AnalyticsCustomers.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsCustomers.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "PlatformAdmin")]
public class AdminController(AppDbContext db) : ControllerBase
{
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await db.Users
            .Select(u => new { u.Id, u.Email, u.FullName, u.Level, u.IsActive, u.CreatedAt, u.OrganizationId })
            .ToListAsync();

        return Ok(users);
    }

    [HttpPatch("users/{id}/toggle-active")]
    public async Task<IActionResult> ToggleActive(Guid id)
    {
        var user = await db.Users.FindAsync(id);
        if (user is null) return NotFound();
        user.IsActive = !user.IsActive;
        await db.SaveChangesAsync();
        return Ok(new { user.Id, user.IsActive });
    }
}
