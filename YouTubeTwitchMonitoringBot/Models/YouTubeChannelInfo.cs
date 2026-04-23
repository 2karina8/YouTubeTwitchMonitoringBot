namespace YouTubeTwitchMonitoringBot.Models;

public class YouTubeChannelInfo
{
    public string ChannelTitle { get; set; } = string.Empty;
    public string ChannelId { get; set; } = string.Empty;
    public ulong Subscribers { get; set; }
    public ulong Views { get; set; }
    public ulong VideoCount { get; set; }

    public string LastVideoTitle { get; set; } = string.Empty;
    public string LastVideoPublishedAt { get; set; } = string.Empty;
    public string LastVideoUrl { get; set; } = string.Empty;
}