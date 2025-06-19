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
        ISongDataModel GetSpecificSong(int songID);
        void AddSongToPlaylist(int playlistID, int songid);
        void RemoveSongFromPlaylist(int playlistID, int songid);
        bool SongPlaylistCheck(int playlistID,int songID);
        IEnumerable<ISongDataModel> GetSongList(int songID);
        IEnumerable<ISongDataModel> SearchSongs(string searchterm);
    }
}
