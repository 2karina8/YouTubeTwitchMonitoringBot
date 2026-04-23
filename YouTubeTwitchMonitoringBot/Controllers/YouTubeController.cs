using Microsoft.AspNetCore.Mvc;
using YouTubeTwitchMonitoringBot.Services;

using Microsoft.AspNetCore.Mvc;
using YouTubeTwitchMonitoringBot.Services;

namespace YouTubeTwitchMonitoringBot.Controllers;

[ApiController]
[Route("api/[controller]")]
public class YouTubeController : ControllerBase
{
    private readonly YouTubeApiService _youtubeService;

    public YouTubeController(YouTubeApiService youtubeService)
    {
        _youtubeService = youtubeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetChannelInfo([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Потрібно ввести назву або посилання на YouTube-канал.");

        var result = await _youtubeService.GetChannelInfoAsync(query);

        if (result == null)
            return NotFound("YouTube-канал не знайдено.");

        return Ok(result);
    }
}