namespace PRISM.Api.Entities;

public class StravaActivity
{
    public int Id { get; set; }
    public long StravaActivityId { get; set; }
    public long AthleteId { get; set; }
    public string Name { get; set; } = "";
    public string SportType { get; set; } = "";
    public DateTime StartDate { get; set; }
    public DateTime StartDateLocal { get; set; }
    public string Timezone { get; set; } = "";
    public double DistanceMeters { get; set; }
    public int MovingTimeSeconds { get; set; }
    public int ElapsedTimeSeconds { get; set; }
    public double TotalElevationGainMeters { get; set; }
    public double AverageSpeedMps { get; set; }
    public double MaxSpeedMps { get; set; }
    public double? AverageWatts { get; set; }
    public double? WeightedAverageWatts { get; set; }
    public double? MaxWatts { get; set; }
    public double? AverageHeartRate { get; set; }
    public double? MaxHeartRate { get; set; }
    public int KudosCount { get; set; }
    public int PrCount { get; set; }
    public int AchievementCount { get; set; }
    public bool Trainer { get; set; }
    public bool Commute { get; set; }
    public bool Manual { get; set; }
    public bool Private { get; set; }
    public string? GearId { get; set; }
    public string? RawJson { get; set; }
    public DateTime LastSyncedUtc { get; set; }
}
