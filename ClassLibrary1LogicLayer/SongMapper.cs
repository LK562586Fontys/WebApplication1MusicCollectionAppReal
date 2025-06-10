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
        private readonly IUserRepository _userRepository;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly ISongRepository _songRepository;
        public SongMapper(ISongRepository songRepository, IPlaylistRepository playlistRepository, IUserRepository userRepository) 
        {
            _userRepository = userRepository;
            _playlistRepository = playlistRepository;
            _songRepository = songRepository;
        }
        public Song FromDataModel(ISongDTO dataModel, List<User> users, List<Playlist> playlists)
        {

            return new Song(_songRepository, _userRepository, _playlistRepository)
            {
                ID = dataModel.ID,
                Name = dataModel.Name,
                Weight = dataModel.Weight,
                DateReleased = dataModel.DateReleased,
                Artist = dataModel.Artist,
                Album = dataModel.Album,
            };
        }
    }
}
