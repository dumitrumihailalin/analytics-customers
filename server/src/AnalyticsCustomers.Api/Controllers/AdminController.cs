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

    [HttpPost("users")]
    public async Task<IActionResult> CreateUser([FromBody] CreateAdminUserRequest req)
    {
        if (await db.Users.AnyAsync(u => u.Email == req.Email))
            return Conflict(new { error = "Email already registered." });

        var adminId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var admin = await db.Users.FindAsync(adminId);

        var user = new User
        {
            Email = req.Email,
            FullName = req.FullName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
            Level = "User",
            OrganizationId = admin?.OrganizationId,
            IsActive = true
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        return Ok(new { user.Id, user.Email, user.FullName, user.Level, user.IsActive, user.OrganizationId });
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
