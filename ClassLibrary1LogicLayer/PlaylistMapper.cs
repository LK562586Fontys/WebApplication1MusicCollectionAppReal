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
        private readonly IPlaylistRepository _playlistRepository;
        private readonly ISongRepository _songRepository;
        private readonly IUserRepository _userRepository;

        public PlaylistMapper(IPlaylistRepository playlistRepository, ISongRepository songRepository, IUserRepository userRepository)
        {
            _playlistRepository = playlistRepository;
            _songRepository = songRepository;
            _userRepository = userRepository;
        }

        public Playlist FromDataModel(IPlaylistDTO dataModel, IEnumerable<IUserDTO> users)
        {

            return new Playlist(_songRepository, _playlistRepository)
            {
                ID = dataModel.ID,
                Name = dataModel.Name,
                Photo = dataModel.Photo,
                Creator = dataModel.Creator
            };
        }
    }
}
