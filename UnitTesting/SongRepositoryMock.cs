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
        public void AddSongToPlaylist(int playlistID, int songid)
        {
            throw new NotImplementedException();
        }

        public void ChangeSongWeight(int songID, int Weight)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ISongDTO> GetSongList(int songID)
        {
            throw new NotImplementedException();
        }

        public ISongDTO GetSpecificSong(int songID)
        {
            throw new NotImplementedException();
        }

        public void RemoveSongFromPlaylist(int playlistID, int songid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ISongDTO> SearchSongs(string searchterm)
        {
            throw new NotImplementedException();
        }
    }
}
