using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public static class SongMapper
    {
        public static Song FromDataModel(
        SongDataModel dataModel,
        List<User> users,
        List<Playlist> playlists)
        {
            var artist = users.FirstOrDefault(u => u.ID == dataModel.artistID);
            var album = playlists.FirstOrDefault(p => p.ID == dataModel.albumID);

            return new Song
            {
                ID = dataModel.ID,
                Name = dataModel.name,
                Weight = dataModel.weight,
                DateReleased = dataModel.dateReleased,
                Artist = artist,
                Album = album
            };
        }
    }
}
