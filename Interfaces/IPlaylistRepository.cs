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
        IEnumerable<IPlaylistDataModel> LoadPlaylists(int playlistID);
        void SortSongs(int playlistID, string field, string order);
        void DeletePlaylist(int playlistID);
        IEnumerable<IPlaylistDataModel> GetPlaylistsByIds(List<int> Playlistids);
        IPlaylistDataModel GetPlaylistById(int playlistID);
        IEnumerable<IPlaylistDataModel> GetAllPlaylists();
    }
}
