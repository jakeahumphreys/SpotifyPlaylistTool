using System.Text.Json.Serialization;

namespace SpotifyPlaylistTool.Settings;

public sealed class AppSettings
{
    public string SpotifyClientId { get; set; }
    public string SpotifyClientSecret { get; set; }
    public string SpotifyUserId { get; set; }
}