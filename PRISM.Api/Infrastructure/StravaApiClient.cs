using System.Net.Http.Headers;
using System.Text.Json;
using PRISM.Api.Dtos;

namespace PRISM.Api.Infrastructure;

public class StravaApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<StravaApiClient> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public StravaApiClient(HttpClient httpClient, ILogger<StravaApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<StravaActivityDto>> GetActivitiesAsync(string accessToken, int page, int perPage)
    {
        var request = new HttpRequestMessage(HttpMethod.Get,
            $"api/v3/athlete/activities?page={page}&per_page={perPage}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            _logger.LogError("Strava activities request failed: {StatusCode} {Body}", response.StatusCode, body);
            response.EnsureSuccessStatusCode();
        }

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<StravaActivityDto>>(json, JsonOptions) ?? new List<StravaActivityDto>();
    }

    public async Task<StravaTokenResponseDto> ExchangeCodeAsync(string clientId, string clientSecret, string code)
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["client_id"] = clientId,
            ["client_secret"] = clientSecret,
            ["code"] = code,
            ["grant_type"] = "authorization_code"
        });

        var response = await _httpClient.PostAsync("oauth/token", content);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            _logger.LogError("Strava token exchange failed: {StatusCode} {Body}", response.StatusCode, body);
            response.EnsureSuccessStatusCode();
        }

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<StravaTokenResponseDto>(json, JsonOptions)
               ?? throw new InvalidOperationException("Failed to deserialize token response");
    }

    public async Task<StravaTokenResponseDto> RefreshTokenAsync(string clientId, string clientSecret, string refreshToken)
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["client_id"] = clientId,
            ["client_secret"] = clientSecret,
            ["refresh_token"] = refreshToken,
            ["grant_type"] = "refresh_token"
        });

        var response = await _httpClient.PostAsync("oauth/token", content);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            _logger.LogError("Strava token refresh failed: {StatusCode} {Body}", response.StatusCode, body);
            response.EnsureSuccessStatusCode();
        }

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<StravaTokenResponseDto>(json, JsonOptions)
               ?? throw new InvalidOperationException("Failed to deserialize token response");
    }
}
