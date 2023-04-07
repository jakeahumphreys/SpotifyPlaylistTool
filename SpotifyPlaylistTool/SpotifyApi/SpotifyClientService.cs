using JSpotifyClient;
using SpotifyPlaylistTool.Settings;
using SpotifyPlaylistTool.SpotifyApi.Types;

namespace SpotifyPlaylistTool.SpotifyApi;

public interface ISpotifyClientService
{
    public Task<PlaylistsForUserResponse> GetPlaylistsForStoredUserId();
}

public sealed class SpotifyClientService : ISpotifyClientService
{
    private readonly ISpotifyClient _spotifyClient;
    private readonly AppSettings _appSettings;

    public SpotifyClientService(ISpotifyClient spotifyClient, AppSettings appSettings)
    {
        _spotifyClient = spotifyClient;
        _appSettings = appSettings;
    }

    public async Task<PlaylistsForUserResponse> GetPlaylistsForStoredUserId()
    {
        var userId = _appSettings.SpotifyUserId;
        var response =  await _spotifyClient.GetPlaylistsForUserId(userId);

        if (response.IsFailure)
        {
            return new PlaylistsForUserResponse().WithError<PlaylistsForUserResponse>(response.Errors.First());
        }

        return new PlaylistsForUserResponse
        {
            PlaylistItems = response.Content.Items
        };
    }
}