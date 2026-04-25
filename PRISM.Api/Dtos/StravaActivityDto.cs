using System.Text.Json.Serialization;

namespace PRISM.Api.Dtos;

public class AthleteRef
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
}

public class StravaActivityDto
{
    [JsonPropertyName("id")]
    public long StravaActivityId { get; set; }

    [JsonPropertyName("athlete")]
    public AthleteRef? Athlete { get; set; }

    public long AthleteId => Athlete?.Id ?? 0;

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("sport_type")]
    public string SportType { get; set; } = "";

    [JsonPropertyName("start_date")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("start_date_local")]
    public DateTime StartDateLocal { get; set; }

    [JsonPropertyName("timezone")]
    public string Timezone { get; set; } = "";

    [JsonPropertyName("distance")]
    public double DistanceMeters { get; set; }

    [JsonPropertyName("moving_time")]
    public int MovingTimeSeconds { get; set; }

    [JsonPropertyName("elapsed_time")]
    public int ElapsedTimeSeconds { get; set; }

    [JsonPropertyName("total_elevation_gain")]
    public double TotalElevationGainMeters { get; set; }

    [JsonPropertyName("average_speed")]
    public double AverageSpeedMps { get; set; }

    [JsonPropertyName("max_speed")]
    public double MaxSpeedMps { get; set; }

    [JsonPropertyName("average_watts")]
    public double? AverageWatts { get; set; }

    [JsonPropertyName("weighted_average_watts")]
    public double? WeightedAverageWatts { get; set; }

    [JsonPropertyName("max_watts")]
    public double? MaxWatts { get; set; }

    [JsonPropertyName("average_heartrate")]
    public double? AverageHeartRate { get; set; }

    [JsonPropertyName("max_heartrate")]
    public double? MaxHeartRate { get; set; }

    [JsonPropertyName("kudos_count")]
    public int KudosCount { get; set; }

    [JsonPropertyName("pr_count")]
    public int PrCount { get; set; }

    [JsonPropertyName("achievement_count")]
    public int AchievementCount { get; set; }

    [JsonPropertyName("trainer")]
    public bool Trainer { get; set; }

    [JsonPropertyName("commute")]
    public bool Commute { get; set; }

    [JsonPropertyName("manual")]
    public bool Manual { get; set; }

    [JsonPropertyName("private")]
    public bool Private { get; set; }

    [JsonPropertyName("gear_id")]
    public string? GearId { get; set; }
}
