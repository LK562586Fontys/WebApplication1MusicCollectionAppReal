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
        private List<ISongDataModel> _songs = new List<ISongDataModel>
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

        public IEnumerable<ISongDataModel> GetSongList(int songID)
        {
            return _songs;
        }

        public ISongDataModel GetSpecificSong(int songID)
        {
            ISongDataModel testsongLOL = new SongDTO { ID = 3 };
            return testsongLOL;
        }

        public void RemoveSongFromPlaylist(int playlistID, int songid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ISongDataModel> SearchSongs(string searchterm)
        {
            throw new NotImplementedException();
        }

        public bool SongPlaylistCheck(int playlistID, int songID)
        {
            throw new NotImplementedException();
        }
        private class SongDTO : ISongDataModel
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int Weight { get; set; }
            public DateTime DateReleased { get; set; }
            public IUserDataModel Artist { get; set; }
            public IPlaylistDataModel Album { get; set; }
        }
    }
}
