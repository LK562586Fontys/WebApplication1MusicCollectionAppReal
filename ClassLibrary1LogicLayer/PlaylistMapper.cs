using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class PlaylistMapper
    {


        public PlaylistMapper()
        {
        }

        public Playlist FromDataModel(IPlaylistDataModel dataModel, List<User> users)
        {
            var fullCreator = users.FirstOrDefault(u => u.ID == dataModel.Creator?.ID);
            return new Playlist(
                dataModel.ID,
                dataModel.Name,
                dataModel.DateAdded,
				dataModel.Photo,
				fullCreator ?? (User)dataModel.Creator
                );
            
        }
    }
}
