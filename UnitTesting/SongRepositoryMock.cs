using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting
{
    public class SongRepositoryMock : ISongRepository
    {
        private List<ISongDTO> _songs = new List<ISongDTO>
        {
            new SongDTO { ID = 1, Name ="fart"},
            new SongDTO { ID = 2, Name ="fart"},
            new SongDTO { ID = 3, Name ="fart"}
        };
        public void AddSongToPlaylist(int playlistID, int songid)
        {
            throw new NotImplementedException();
        }

        public void ChangeSongWeight(int songID, int Weight)
        {
            return;
        }

        public IEnumerable<ISongDTO> GetSongList(int songID)
        {
            return _songs;
        }

        public ISongDTO GetSpecificSong(int songID)
        {
            ISongDTO testsongLOL = new SongDTO { ID = 3 };
            return testsongLOL;
        }

        public void RemoveSongFromPlaylist(int playlistID, int songid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ISongDTO> SearchSongs(string searchterm)
        {
            throw new NotImplementedException();
        }

        public bool SongPlaylistCheck(int playlistID, int songID)
        {
            throw new NotImplementedException();
        }
        private class SongDTO : ISongDTO
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int Weight { get; set; }
            public DateTime DateReleased { get; set; }
            public IUserDTO Artist { get; set; }
            public IPlaylistDTO Album { get; set; }
        }
    }
}
