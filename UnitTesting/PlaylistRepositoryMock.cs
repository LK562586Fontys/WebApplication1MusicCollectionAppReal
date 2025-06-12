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
        private List<IPlaylistDTO> _playlist = new List<IPlaylistDTO>
        {
        new PlaylistDTO { ID = 1, Name = "Alice" },
        new PlaylistDTO { ID = 2, Name = "Bob" }
        };
        public void DeletePlaylist(int playlistID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPlaylistDTO> GetAllPlaylists()
        {
            return _playlist;
        }

        public IPlaylistDTO GetPlaylistById(int playlistID)
        {
            if (playlistID == 1)
            {
                return new PlaylistDTO { ID = 1, Name = "TestPlaylist" };
            }
            return null;
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
            return;
        }

        public void UpdatePlaylistPhoto(int playlistID, byte[] newPhoto)
        {
            return;
        }
        private class PlaylistDTO : IPlaylistDTO
        {
            public int ID {  get; set; }
            public string Name {  get; set; }
            public DateTime DateAdded { get; set; }
            public byte[]? Photo { get; set; }
            public IUserDTO Creator { get; set; }
        }
    }
}
