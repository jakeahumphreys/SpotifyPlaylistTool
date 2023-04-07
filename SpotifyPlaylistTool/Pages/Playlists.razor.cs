using JSpotifyClient.Types;
using Microsoft.AspNetCore.Components;
using SpotifyPlaylistTool.SpotifyApi;

namespace SpotifyPlaylistTool.Pages;

public partial class Playlists
{
    [Inject] 
    protected ISpotifyClientService SpotifyClientService { get; set; }
    
    public List<PlaylistItem> UserPlaylists { get; set; }
    public string ErrorMessage { get; set; }

    protected override async Task OnInitializedAsync()
    {
        UserPlaylists = new List<PlaylistItem>();

        var userPlaylistsResult = await SpotifyClientService.GetPlaylistsForStoredUserId();

        if (userPlaylistsResult.Error != null)
        {
            ErrorMessage = userPlaylistsResult.Error.Message;
        }

        UserPlaylists = userPlaylistsResult.PlaylistItems;
    }
    
}