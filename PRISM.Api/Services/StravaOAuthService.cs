using Microsoft.EntityFrameworkCore;
using PRISM.Api.Entities;
using PRISM.Api.Infrastructure;

namespace PRISM.Api.Services;

public class StravaOAuthService
{
    private readonly PrismDbContext _db;
    private readonly StravaApiClient _stravaClient;
    private readonly IConfiguration _config;
    private readonly ILogger<StravaOAuthService> _logger;

    public StravaOAuthService(
        PrismDbContext db,
        StravaApiClient stravaClient,
        IConfiguration config,
        ILogger<StravaOAuthService> logger)
    {
        _db = db;
        _stravaClient = stravaClient;
        _config = config;
        _logger = logger;
    }

    public string GetAuthorizationUrl(string clientId, string redirectUri)
    {
        var scope = "read,activity:read_all";
        return $"https://www.strava.com/oauth/authorize?client_id={clientId}&redirect_uri={Uri.EscapeDataString(redirectUri)}&response_type=code&scope={scope}";
    }

    public async Task<StravaAthlete> HandleCallbackAsync(string code)
    {
        var clientId = _config["Strava:ClientId"] ?? throw new InvalidOperationException("Strava ClientId not configured");
        var clientSecret = _config["Strava:ClientSecret"] ?? throw new InvalidOperationException("Strava ClientSecret not configured");

        var tokenResponse = await _stravaClient.ExchangeCodeAsync(clientId, clientSecret, code);

        var athlete = await _db.Athletes.FirstOrDefaultAsync(a => a.StravaAthleteId == tokenResponse.AthleteId);
        var now = DateTime.UtcNow;

        if (athlete is null)
        {
            athlete = new StravaAthlete
            {
                StravaAthleteId = tokenResponse.AthleteId,
                AccessToken = tokenResponse.AccessToken,
                RefreshToken = tokenResponse.RefreshToken,
                TokenExpiresAt = DateTimeOffset.FromUnixTimeSeconds(tokenResponse.ExpiresAt).UtcDateTime,
                CreatedUtc = now,
                UpdatedUtc = now
            };
            _db.Athletes.Add(athlete);
        }
        else
        {
            athlete.AccessToken = tokenResponse.AccessToken;
            athlete.RefreshToken = tokenResponse.RefreshToken;
            athlete.TokenExpiresAt = DateTimeOffset.FromUnixTimeSeconds(tokenResponse.ExpiresAt).UtcDateTime;
            athlete.UpdatedUtc = now;
        }

        await _db.SaveChangesAsync();
        _logger.LogInformation("Stored tokens for Strava athlete {AthleteId}", tokenResponse.AthleteId);
        return athlete;
    }

    public async Task<string> GetValidAccessTokenAsync(long stravaAthleteId)
    {
        var athlete = await _db.Athletes.FirstOrDefaultAsync(a => a.StravaAthleteId == stravaAthleteId)
                      ?? throw new InvalidOperationException($"Athlete {stravaAthleteId} not found");

        if (DateTime.UtcNow < athlete.TokenExpiresAt.AddMinutes(-5))
            return athlete.AccessToken;

        _logger.LogInformation("Refreshing access token for athlete {AthleteId}", stravaAthleteId);

        var clientId = _config["Strava:ClientId"] ?? throw new InvalidOperationException("Strava ClientId not configured");
        var clientSecret = _config["Strava:ClientSecret"] ?? throw new InvalidOperationException("Strava ClientSecret not configured");

        var tokenResponse = await _stravaClient.RefreshTokenAsync(clientId, clientSecret, athlete.RefreshToken);

        athlete.AccessToken = tokenResponse.AccessToken;
        athlete.RefreshToken = tokenResponse.RefreshToken;
        athlete.TokenExpiresAt = DateTimeOffset.FromUnixTimeSeconds(tokenResponse.ExpiresAt).UtcDateTime;
        athlete.UpdatedUtc = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return athlete.AccessToken;
    }
}
