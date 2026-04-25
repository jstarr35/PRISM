using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRISM.Api.Dtos;
using PRISM.Api.Infrastructure;
using PRISM.Api.Services;

namespace PRISM.Api.Controllers;

[ApiController]
[Route("api/sync")]
public class StravaSyncController : ControllerBase
{
    private readonly StravaActivitySyncService _syncService;
    private readonly PrismDbContext _db;
    private readonly ILogger<StravaSyncController> _logger;

    public StravaSyncController(
        StravaActivitySyncService syncService,
        PrismDbContext db,
        ILogger<StravaSyncController> logger)
    {
        _syncService = syncService;
        _db = db;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<SyncStatusDto>> TriggerSync()
    {
        var athlete = await _db.Athletes.FirstOrDefaultAsync();
        if (athlete is null)
            return BadRequest("No athlete found. Please connect your Strava account first.");

        var syncRun = await _syncService.SyncActivitiesAsync(athlete.StravaAthleteId);

        return Ok(new SyncStatusDto
        {
            LastSyncRun = new SyncRunDto
            {
                StartedUtc = syncRun.StartedUtc,
                CompletedUtc = syncRun.CompletedUtc,
                Status = syncRun.Status,
                ActivitiesProcessed = syncRun.ActivitiesProcessed,
                ErrorMessage = syncRun.ErrorMessage
            }
        });
    }

    [HttpGet("status")]
    public async Task<ActionResult<SyncStatusDto>> GetStatus()
    {
        var lastRun = await _db.SyncRuns
            .OrderByDescending(r => r.StartedUtc)
            .FirstOrDefaultAsync();

        return Ok(new SyncStatusDto
        {
            LastSyncRun = lastRun is null ? null : new SyncRunDto
            {
                StartedUtc = lastRun.StartedUtc,
                CompletedUtc = lastRun.CompletedUtc,
                Status = lastRun.Status,
                ActivitiesProcessed = lastRun.ActivitiesProcessed,
                ErrorMessage = lastRun.ErrorMessage
            }
        });
    }
}
