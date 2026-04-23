namespace YouTubeTwitchMonitoringBot.Models;

public class TwitchStreamerInfo
{
    public string UserName { get; set; } = string.Empty;
    public bool IsLive { get; set; }
    public string GameName { get; set; } = string.Empty;
    public int ViewerCount { get; set; }
    public string Language { get; set; } = string.Empty;
    public string StreamTitle { get; set; } = string.Empty;
}
