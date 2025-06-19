using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class SongMapper
    {
        public SongMapper() 
        {
        }
        public Song FromDataModel(ISongDataModel dataModel, List<User> users, List<Playlist> playlists)
        {
            var artist = users.FirstOrDefault(u => u.ID == dataModel.Artist?.ID);
            var album = playlists.FirstOrDefault(p => p.ID == dataModel.Album?.ID);

            return new Song(
                dataModel.ID,
                dataModel.Name,
                dataModel.DateReleased,
                dataModel.Weight,
                artist,
                album
                );
        }
    }
}
