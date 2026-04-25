using Microsoft.AspNetCore.Mvc;
using PRISM.Api.Services;

namespace PRISM.Api.Controllers;

[ApiController]
[Route("api/auth/strava")]
public class StravaAuthController : ControllerBase
{
    private readonly StravaOAuthService _oAuthService;
    private readonly IConfiguration _config;
    private readonly ILogger<StravaAuthController> _logger;

    public StravaAuthController(
        StravaOAuthService oAuthService,
        IConfiguration config,
        ILogger<StravaAuthController> logger)
    {
        _oAuthService = oAuthService;
        _config = config;
        _logger = logger;
    }

    [HttpGet("connect")]
    public IActionResult Connect()
    {
        var clientId = _config["Strava:ClientId"] ?? "";
        var redirectUri = _config["Strava:RedirectUri"] ?? "http://localhost:5000/api/auth/strava/callback";
        var url = _oAuthService.GetAuthorizationUrl(clientId, redirectUri);
        return Redirect(url);
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback([FromQuery] string code)
    {
        if (string.IsNullOrEmpty(code))
            return BadRequest("Missing code parameter");

        try
        {
            await _oAuthService.HandleCallbackAsync(code);
            return Redirect("http://localhost:4200/dashboard");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "OAuth callback failed");
            return StatusCode(500, "Authentication failed");
        }
    }
}
