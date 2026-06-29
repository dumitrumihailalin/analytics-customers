using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnalyticsCustomers.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    [HttpPost("bulk")]
    public IActionResult SubmitBulk() =>
        StatusCode(501, new { error = "Orders endpoint pending reimplementation for the new schema." });
}
