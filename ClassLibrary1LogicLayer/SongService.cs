using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class SongService : ISongService
    {
        private readonly ISongRepository _songRepo;
        private readonly IUserRepository _userRepo;
        private readonly IPlaylistRepository _playlistRepo;
        private readonly SongMapper _songMapper;
        private readonly PlaylistMapper _playlistMapper;

        public SongService(ISongRepository songRepo, IUserRepository userRepo, IPlaylistRepository playlistRepo)
        {
            _songRepo = songRepo;
            _userRepo = userRepo;
            _playlistRepo = playlistRepo;
            _songMapper = new SongMapper(songRepo, playlistRepo, userRepo);
            _playlistMapper = new PlaylistMapper(playlistRepo, songRepo, userRepo);
        }

        public ISongDTO? GetSongById(int id)
        {
            var dto = _songRepo.GetSpecificSong(id);
            if (dto == null) return null;

			var users = _userRepo.GetAllUsers()
	                .Select(u => new User(
				        id: u.ID,
			            name: u.Name,
			            emailAddress: u.EmailAddress,
			            passwordHash: u.PasswordHash,
			            joinDate: u.JoinDate,
			            profilePhoto: u.ProfilePhoto
			        )).ToList();

			var playlists = _playlistRepo.GetAllPlaylists()
                    .Select(p => _playlistMapper.FromDataModel(p, users))
                    .ToList();

            return (ISongDTO)_songMapper.FromDataModel(dto, users, playlists);
        }

        public List<Song> GetAllSongs(int playlistid)
        {
            var songDtos = _songRepo.GetSongList(playlistid).ToList();

			var users = _userRepo.GetAllUsers()
					.Select(u => new User(
						id: u.ID,
						name: u.Name,
						emailAddress: u.EmailAddress,
						passwordHash: u.PasswordHash,
						joinDate: u.JoinDate,
						profilePhoto: u.ProfilePhoto
					)).ToList();

			var playlists = _playlistRepo.GetAllPlaylists()
					.Select(p => _playlistMapper.FromDataModel(p, users))
					.ToList();

			return songDtos.Select(dto => _songMapper.FromDataModel(dto, users, playlists)).ToList();
        }
    }
}
