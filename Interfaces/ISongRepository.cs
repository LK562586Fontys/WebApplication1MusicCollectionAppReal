using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface ISongRepository
    {
        void ChangeSongWeight(int songID, int Weight);
        ISongDTO GetSpecificSong(int songID);
        void AddSongToPlaylist(int playlistID, int songid);
        void RemoveSongFromPlaylist(int playlistID, int songid);
        IEnumerable<ISongDTO> GetSongList(int songID);
        IEnumerable<ISongDTO> SearchSongs(string searchterm);
    }
}
