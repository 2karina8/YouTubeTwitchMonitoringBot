using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using YouTubeTwitchMonitoringBot.Models;

namespace YouTubeTwitchMonitoringBot.Services;

public class YouTubeApiService
{
    private readonly Google.Apis.YouTube.v3.YouTubeService _youtube;
    public YouTubeApiService(IConfiguration configuration)
    {
        string apiKey = configuration["ApiKeys:YouTubeApiKey"] ?? "";
        _youtube = new Google.Apis.YouTube.v3.YouTubeService(
            new BaseClientService.Initializer
            {
                ApiKey = apiKey,
                ApplicationName = "YouTubeTwitchMonitoringBot"
            });
    }
    public async Task<YouTubeChannelInfo?> GetChannelInfoAsync(string query)
    {
        string? channelId = await ResolveChannelIdAsync(query);

        if (string.IsNullOrWhiteSpace(channelId))
            return null;

        var channelRequest = _youtube.Channels.List("snippet,statistics,contentDetails");
        channelRequest.Id = channelId;

        var channelResponse = await channelRequest.ExecuteAsync();
        var channel = channelResponse.Items.FirstOrDefault();

        if (channel == null)
            return null;

        string uploadsPlaylistId = channel.ContentDetails.RelatedPlaylists.Uploads;

        var playlistRequest = _youtube.PlaylistItems.List("snippet");
        playlistRequest.PlaylistId = uploadsPlaylistId;
        playlistRequest.MaxResults = 1;

        var playlistResponse = await playlistRequest.ExecuteAsync();
        var lastVideo = playlistResponse.Items.FirstOrDefault();

        return new YouTubeChannelInfo
        {
            ChannelTitle = channel.Snippet.Title,
            ChannelId = channel.Id,
            Subscribers = channel.Statistics.SubscriberCount ?? 0,
            Views = channel.Statistics.ViewCount ?? 0,
            VideoCount = channel.Statistics.VideoCount ?? 0,
            LastVideoTitle = lastVideo?.Snippet.Title ?? "Немає даних",
            LastVideoPublishedAt = lastVideo?.Snippet.PublishedAtDateTimeOffset?.ToString("yyyy-MM-dd HH:mm") ?? "Немає даних",
            LastVideoUrl = lastVideo?.Snippet.ResourceId?.VideoId is string videoId
                ? $"https://www.youtube.com/watch?v={videoId}"
                : "Немає даних"
        };
    }

    private async Task<string?> ResolveChannelIdAsync(string query)
    {
        if (query.Contains("/channel/"))
        {
            var parts = query.Split("/channel/");
            return parts.Length > 1 ? parts[1].Split('/', '?')[0] : null;
        }

        var searchRequest = _youtube.Search.List("snippet");
        searchRequest.Q = query;
        searchRequest.Type = "channel";
        searchRequest.MaxResults = 1;

        var searchResponse = await searchRequest.ExecuteAsync();
        return searchResponse.Items.FirstOrDefault()?.Snippet.ChannelId;
    }
}
