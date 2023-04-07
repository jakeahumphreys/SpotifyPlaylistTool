using JCommon.Communication.External;
using JSpotifyClient.Types;

namespace SpotifyPlaylistTool.SpotifyApi.Types;

public sealed class PlaylistsForUserResponse : BaseResponse
{
    public List<PlaylistItem> PlaylistItems { get; set; }
}