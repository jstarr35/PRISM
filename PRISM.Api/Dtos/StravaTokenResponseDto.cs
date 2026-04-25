using System.Text.Json.Serialization;

namespace PRISM.Api.Dtos;

public class StravaTokenAthleteRef
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
}

public class StravaTokenResponseDto
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = "";

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; } = "";

    [JsonPropertyName("expires_at")]
    public long ExpiresAt { get; set; }

    [JsonPropertyName("athlete")]
    public StravaTokenAthleteRef? Athlete { get; set; }

    public long AthleteId => Athlete?.Id ?? 0;
}
