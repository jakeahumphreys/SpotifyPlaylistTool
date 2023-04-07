using JSpotifyClient;
using JSpotifyClient.Types;

namespace SpotifyPlaylistTool.SpotifyApi;

public sealed class SpotifyClientService
{
    private readonly ISpotifyClient _spotifyClient;

    public SpotifyClientService(ISpotifyClient spotifyClient)
    {
        _spotifyClient = spotifyClient;
    }

    public async Task<List<PlaylistItem>> GetPlaylistsForUserId(string userId)
    {
        var response = await _spotifyClient.GetPlaylistsForUserId(userId);

        if (response.IsFailure)
        {
            //TODO handle error
        }

        return response.Content.Items;
    }
}