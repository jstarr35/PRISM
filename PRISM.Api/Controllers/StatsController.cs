using Microsoft.AspNetCore.Mvc;
using PRISM.Api.Dtos;
using PRISM.Api.Services;

namespace PRISM.Api.Controllers;

[ApiController]
[Route("api/stats")]
public class StatsController : ControllerBase
{
    private readonly AllTimeStatsService _statsService;

    public StatsController(AllTimeStatsService statsService)
    {
        _statsService = statsService;
    }

    [HttpGet("all-time")]
    public async Task<ActionResult<List<StatCardDto>>> GetAllTimeStats()
    {
        var stats = await _statsService.GetAllTimeStatsAsync();
        return Ok(stats);
    }
}
