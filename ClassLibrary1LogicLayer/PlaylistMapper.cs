using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public static class PlaylistMapper
    {
        public static Playlist FromDataModel(PlaylistDataModel dataModel, List<User> users)
        {
            var creator = users.FirstOrDefault(u => u.ID == dataModel.Creator);

            return new Playlist
            {
                ID = dataModel.ID,
                Name = dataModel.Name,
                Photo = dataModel.Photo,
                Creator = creator
            };
        }
    }
}
