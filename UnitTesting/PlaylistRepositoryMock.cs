using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting
{
    public class PlaylistRepositoryMock : IPlaylistRepository
    {
        public void DeletePlaylist(int playlistID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPlaylistDTO> GetAllPlaylists()
        {
            throw new NotImplementedException();
        }

        public IPlaylistDTO GetPlaylistById(int playlistID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPlaylistDTO> GetPlaylistsByIds(List<int> Playlistids)
        {
            throw new NotImplementedException();
        }

        public void InsertPlaylist(string generatedName, DateTime CurrentDate, int userID)
        {
            return;
        }

        public IEnumerable<IPlaylistDTO> LoadPlaylists(int playlistID)
        {
            throw new NotImplementedException();
        }

        public void SortSongs(int playlistID, string field, string order)
        {
            throw new NotImplementedException();
        }

        public void UpdatePlaylistName(int playlistID, string newName)
        {
            throw new NotImplementedException();
        }

        public void UpdatePlaylistPhoto(int playlistID, byte[] newPhoto)
        {
            throw new NotImplementedException();
        }
    }
}
