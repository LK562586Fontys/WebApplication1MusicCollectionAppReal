using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IPlaylistRepository
    {
        void InsertPlaylist(string generatedName, DateTime CurrentDate, int userID);
        void UpdatePlaylistPhoto(int playlistID, byte[] newPhoto);
        void UpdatePlaylistName(int playlistID, string newName);
        IEnumerable<IPlaylistDTO> LoadPlaylists(int playlistID);
        void SortSongs(int playlistID, string field, string order);
        void DeletePlaylist(int playlistID);
        IEnumerable<IPlaylistDTO> GetPlaylistsByIds(List<int> Playlistids);
        IPlaylistDTO GetPlaylistById(int playlistID);
        IEnumerable<IPlaylistDTO> GetAllPlaylists();
    }
}
