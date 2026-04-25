namespace PRISM.Api.Entities;

public class StravaAthlete
{
    public int Id { get; set; }
    public long StravaAthleteId { get; set; }
    public string AccessToken { get; set; } = "";
    public string RefreshToken { get; set; } = "";
    public DateTime TokenExpiresAt { get; set; }
    public DateTime CreatedUtc { get; set; }
    public DateTime UpdatedUtc { get; set; }
}
