using Microsoft.EntityFrameworkCore;
using PRISM.Api.Dtos;
using PRISM.Api.Entities;
using PRISM.Api.Infrastructure;

namespace PRISM.Api.Services;

public class AllTimeStatsService
{
    private readonly PrismDbContext _db;

    public AllTimeStatsService(PrismDbContext db)
    {
        _db = db;
    }

    public async Task<List<StatCardDto>> GetAllTimeStatsAsync()
    {
        var activities = await _db.Activities.ToListAsync();
        var stats = new List<StatCardDto>();

        AddStat(stats, activities, "fastest_speed", "Fastest Recorded Speed",
            a => a.MaxSpeedMps,
            v => v * 2.23694,
            "mph",
            v => $"{v:F1} mph");

        AddStat(stats, activities, "longest_distance", "Longest Distance",
            a => a.DistanceMeters,
            v => v / 1609.344,
            "miles",
            v => $"{v:F2} mi");

        AddStat(stats, activities, "longest_moving_time", "Longest Moving Time",
            a => (double)a.MovingTimeSeconds,
            v => v,
            "seconds",
            v =>
            {
                var ts = TimeSpan.FromSeconds(v);
                return $"{(int)ts.TotalHours}:{ts.Minutes:D2}";
            });

        AddStat(stats, activities, "biggest_elevation_gain", "Biggest Elevation Gain",
            a => a.TotalElevationGainMeters,
            v => v * 3.28084,
            "feet",
            v => $"{v:F0} ft");

        AddStat(stats, activities, "highest_avg_speed", "Highest Average Speed",
            a => a.AverageSpeedMps,
            v => v * 2.23694,
            "mph",
            v => $"{v:F1} mph");

        AddNullableStat(stats, activities, "highest_avg_watts", "Highest Average Watts",
            a => a.AverageWatts,
            v => v,
            "watts",
            v => $"{v:F0} W");

        AddNullableStat(stats, activities, "highest_weighted_avg_watts", "Highest Weighted Avg Watts",
            a => a.WeightedAverageWatts,
            v => v,
            "watts",
            v => $"{v:F0} W");

        AddNullableStat(stats, activities, "highest_max_watts", "Highest Max Watts",
            a => a.MaxWatts,
            v => v,
            "watts",
            v => $"{v:F0} W");

        AddNullableStat(stats, activities, "highest_avg_heart_rate", "Highest Average Heart Rate",
            a => a.AverageHeartRate,
            v => v,
            "bpm",
            v => $"{v:F0} bpm");

        AddNullableStat(stats, activities, "highest_max_heart_rate", "Highest Max Heart Rate",
            a => a.MaxHeartRate,
            v => v,
            "bpm",
            v => $"{v:F0} bpm");

        AddStat(stats, activities, "most_kudos", "Most Kudos",
            a => (double)a.KudosCount,
            v => v,
            "kudos",
            v => $"{(int)v} kudos");

        AddStat(stats, activities, "most_prs", "Most PRs",
            a => (double)a.PrCount,
            v => v,
            "PRs",
            v => $"{(int)v} PRs");

        AddStat(stats, activities, "most_achievements", "Most Achievements",
            a => (double)a.AchievementCount,
            v => v,
            "achievements",
            v => $"{(int)v} achievements");

        return stats;
    }

    private static void AddStat(
        List<StatCardDto> stats,
        List<StravaActivity> activities,
        string key,
        string label,
        Func<StravaActivity, double> selector,
        Func<double, double> converter,
        string unit,
        Func<double, string> formatter)
    {
        if (activities.Count == 0) return;

        var best = activities.MaxBy(selector);
        if (best is null) return;

        var raw = selector(best);
        var converted = converter(raw);

        stats.Add(new StatCardDto
        {
            Key = key,
            Label = label,
            Value = converted,
            Unit = unit,
            DisplayValue = formatter(converted),
            ActivityId = best.StravaActivityId,
            ActivityName = best.Name,
            SportType = best.SportType,
            StartDateLocal = best.StartDateLocal,
            StravaActivityUrl = $"https://www.strava.com/activities/{best.StravaActivityId}"
        });
    }

    private static void AddNullableStat(
        List<StatCardDto> stats,
        List<StravaActivity> activities,
        string key,
        string label,
        Func<StravaActivity, double?> selector,
        Func<double, double> converter,
        string unit,
        Func<double, string> formatter)
    {
        var withValue = activities.Where(a => selector(a).HasValue).ToList();
        if (withValue.Count == 0) return;

        var best = withValue.MaxBy(a => selector(a)!.Value);
        if (best is null) return;

        var raw = selector(best)!.Value;
        var converted = converter(raw);

        stats.Add(new StatCardDto
        {
            Key = key,
            Label = label,
            Value = converted,
            Unit = unit,
            DisplayValue = formatter(converted),
            ActivityId = best.StravaActivityId,
            ActivityName = best.Name,
            SportType = best.SportType,
            StartDateLocal = best.StartDateLocal,
            StravaActivityUrl = $"https://www.strava.com/activities/{best.StravaActivityId}"
        });
    }
}
