using System.Security.Claims;
using AnalyticsCustomers.Api.DTOs;
using AnalyticsCustomers.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnalyticsCustomers.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AnalyticsController(AnalyticsService analyticsService) : ControllerBase
{
    [HttpGet("dashboard")]
    public async Task<ActionResult<DashboardSummaryDto>> GetDashboard([FromQuery] int? year)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Ok(await analyticsService.GetDashboardAsync(userId, year));
    }

    [HttpGet("admin/stats")]
    [Authorize(Roles = "PlatformAdmin")]
    public async Task<ActionResult<AdminStatsDto>> GetAdminStats() =>
        Ok(await analyticsService.GetAdminStatsAsync());
}
