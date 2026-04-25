using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PRISM.Api.Entities;
using PRISM.Api.Infrastructure;

namespace PRISM.Api.Services;

public class StravaActivitySyncService
{
    private readonly PrismDbContext _db;
    private readonly StravaApiClient _stravaClient;
    private readonly StravaOAuthService _oAuthService;
    private readonly ILogger<StravaActivitySyncService> _logger;

    public StravaActivitySyncService(
        PrismDbContext db,
        StravaApiClient stravaClient,
        StravaOAuthService oAuthService,
        ILogger<StravaActivitySyncService> logger)
    {
        _db = db;
        _stravaClient = stravaClient;
        _oAuthService = oAuthService;
        _logger = logger;
    }

    public async Task<StravaSyncRun> SyncActivitiesAsync(long stravaAthleteId)
    {
        var syncRun = new StravaSyncRun
        {
            StartedUtc = DateTime.UtcNow,
            Status = "Running"
        };
        _db.SyncRuns.Add(syncRun);
        await _db.SaveChangesAsync();

        try
        {
            var accessToken = await _oAuthService.GetValidAccessTokenAsync(stravaAthleteId);

            int page = 1;
            const int perPage = 200;
            int totalProcessed = 0;

            while (true)
            {
                var activities = await _stravaClient.GetActivitiesAsync(accessToken, page, perPage);

                if (activities.Count == 0)
                    break;

                foreach (var dto in activities)
                {
                    var existing = await _db.Activities.FirstOrDefaultAsync(a => a.StravaActivityId == dto.StravaActivityId);
                    var rawJson = JsonSerializer.Serialize(dto);
                    var now = DateTime.UtcNow;

                    if (existing is null)
                    {
                        _db.Activities.Add(new StravaActivity
                        {
                            StravaActivityId = dto.StravaActivityId,
                            AthleteId = dto.AthleteId,
                            Name = dto.Name,
                            SportType = dto.SportType,
                            StartDate = dto.StartDate,
                            StartDateLocal = dto.StartDateLocal,
                            Timezone = dto.Timezone,
                            DistanceMeters = dto.DistanceMeters,
                            MovingTimeSeconds = dto.MovingTimeSeconds,
                            ElapsedTimeSeconds = dto.ElapsedTimeSeconds,
                            TotalElevationGainMeters = dto.TotalElevationGainMeters,
                            AverageSpeedMps = dto.AverageSpeedMps,
                            MaxSpeedMps = dto.MaxSpeedMps,
                            AverageWatts = dto.AverageWatts,
                            WeightedAverageWatts = dto.WeightedAverageWatts,
                            MaxWatts = dto.MaxWatts,
                            AverageHeartRate = dto.AverageHeartRate,
                            MaxHeartRate = dto.MaxHeartRate,
                            KudosCount = dto.KudosCount,
                            PrCount = dto.PrCount,
                            AchievementCount = dto.AchievementCount,
                            Trainer = dto.Trainer,
                            Commute = dto.Commute,
                            Manual = dto.Manual,
                            Private = dto.Private,
                            GearId = dto.GearId,
                            RawJson = rawJson,
                            LastSyncedUtc = now
                        });
                    }
                    else
                    {
                        existing.AthleteId = dto.AthleteId;
                        existing.Name = dto.Name;
                        existing.SportType = dto.SportType;
                        existing.StartDate = dto.StartDate;
                        existing.StartDateLocal = dto.StartDateLocal;
                        existing.Timezone = dto.Timezone;
                        existing.DistanceMeters = dto.DistanceMeters;
                        existing.MovingTimeSeconds = dto.MovingTimeSeconds;
                        existing.ElapsedTimeSeconds = dto.ElapsedTimeSeconds;
                        existing.TotalElevationGainMeters = dto.TotalElevationGainMeters;
                        existing.AverageSpeedMps = dto.AverageSpeedMps;
                        existing.MaxSpeedMps = dto.MaxSpeedMps;
                        existing.AverageWatts = dto.AverageWatts;
                        existing.WeightedAverageWatts = dto.WeightedAverageWatts;
                        existing.MaxWatts = dto.MaxWatts;
                        existing.AverageHeartRate = dto.AverageHeartRate;
                        existing.MaxHeartRate = dto.MaxHeartRate;
                        existing.KudosCount = dto.KudosCount;
                        existing.PrCount = dto.PrCount;
                        existing.AchievementCount = dto.AchievementCount;
                        existing.Trainer = dto.Trainer;
                        existing.Commute = dto.Commute;
                        existing.Manual = dto.Manual;
                        existing.Private = dto.Private;
                        existing.GearId = dto.GearId;
                        existing.RawJson = rawJson;
                        existing.LastSyncedUtc = now;
                    }

                    totalProcessed++;
                }

                await _db.SaveChangesAsync();
                _logger.LogInformation("Synced page {Page}, {Count} activities", page, activities.Count);

                if (activities.Count < perPage)
                    break;

                page++;
            }

            syncRun.Status = "Completed";
            syncRun.CompletedUtc = DateTime.UtcNow;
            syncRun.ActivitiesProcessed = totalProcessed;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sync failed for athlete {AthleteId}", stravaAthleteId);
            syncRun.Status = "Failed";
            syncRun.CompletedUtc = DateTime.UtcNow;
            syncRun.ErrorMessage = ex.Message;
        }

        await _db.SaveChangesAsync();
        return syncRun;
    }
}
