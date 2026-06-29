using System.Security.Claims;
using AnalyticsCustomers.Api.Data;
using AnalyticsCustomers.Api.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnalyticsCustomers.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ProfileResponse>> GetProfile()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await db.Users.FindAsync(userId);
        if (user is null) return NotFound();

        return Ok(new ProfileResponse(user.Id, user.Email, user.FullName, user.Level, user.CreatedAt));
    }

    [HttpPut]
    public async Task<ActionResult<ProfileResponse>> UpdateProfile(ProfileRequest req)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await db.Users.FindAsync(userId);
        if (user is null) return NotFound();

        user.FullName = req.FullName;
        await db.SaveChangesAsync();

        return Ok(new ProfileResponse(user.Id, user.Email, user.FullName, user.Level, user.CreatedAt));
    }
}
