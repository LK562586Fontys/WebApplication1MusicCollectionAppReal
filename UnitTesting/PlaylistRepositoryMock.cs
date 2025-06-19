using Interfaces;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting
{
    public class PlaylistRepositoryMock : IPlaylistRepository
    {
        private List<IPlaylistDataModel> _playlist = new List<IPlaylistDataModel>
        {
        new PlaylistDTO { ID = 1, Name = "Alice" },
        new PlaylistDTO { ID = 2, Name = "Bob" }
        };
        public void DeletePlaylist(int playlistID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPlaylistDataModel> GetAllPlaylists()
        {
            return _playlist;
        }

        public IPlaylistDataModel GetPlaylistById(int playlistID)
        {
            if (playlistID == 1)
            {
                return new PlaylistDTO { ID = 1, Name = "TestPlaylist" };
            }
            return null;
        }

        public IEnumerable<IPlaylistDataModel> GetPlaylistsByIds(List<int> Playlistids)
        {
            throw new NotImplementedException();
        }

        public void InsertPlaylist(string generatedName, DateTime CurrentDate, int userID)
        {
            return;
        }

        public IEnumerable<IPlaylistDataModel> LoadPlaylists(int playlistID)
        {
            throw new NotImplementedException();
        }

        public void SortSongs(int playlistID, string field, string order)
        {
            throw new NotImplementedException();
        }

        public void UpdatePlaylistName(int playlistID, string newName)
        {
            return;
        }

        public void UpdatePlaylistPhoto(int playlistID, byte[] newPhoto)
        {
            return;
        }
        private class PlaylistDTO : IPlaylistDataModel
        {
            public int ID {  get; set; }
            public string Name {  get; set; }
            public DateTime DateAdded { get; set; }
            public byte[]? Photo { get; set; }
            public IUserDataModel Creator { get; set; }
        }
    }
}
