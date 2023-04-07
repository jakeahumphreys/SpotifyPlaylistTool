using JSpotifyClient;
using JSpotifyClient.Types;
using SpotifyPlaylistTool.Settings;

namespace SpotifyPlaylistTool.SpotifyApi;

public sealed class SpotifyClientService
{
    private readonly ISpotifyClient _spotifyClient;
    private readonly AppSettings _appSettings;

    public SpotifyClientService(ISpotifyClient spotifyClient, AppSettings appSettings)
    {
        _spotifyClient = spotifyClient;
        _appSettings = appSettings;
    }

    public async Task<List<PlaylistItem>> GetPlaylistsForStoredUserId()
    {
        var userId = _appSettings.SpotifyUserId;
        var response = await _spotifyClient.GetPlaylistsForUserId(userId);

        if (response.IsFailure)
        {
            //TODO handle error
        }

        return response.Content.Items;
    }
}