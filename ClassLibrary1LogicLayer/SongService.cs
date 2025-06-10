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

        public SongService(ISongRepository songRepo, IUserRepository userRepo, IPlaylistRepository playlistRepo)
        {
            _songRepo = songRepo;
            _userRepo = userRepo;
            _playlistRepo = playlistRepo;
            _songMapper = new SongMapper(songRepo, playlistRepo, userRepo);
        }

        public ISongDTO? GetSongById(int id)
        {
            var dto = _songRepo.GetSpecificSong(id);
            if (dto == null) return null;

            var users = _userRepo.GetAllUsers()
                         .Select(u => new User(_userRepo)
                         {
                             ID = u.ID,
                             Name = u.Name,
                             EmailAddress = u.EmailAddress,
                             PasswordHash = u.PasswordHash
                         })
                         .ToList();

            var playlists = _playlistRepo.GetAllPlaylists()
                                        .Select(p => new Playlist(_songRepo, _playlistRepo)
                                        {
                                            ID = p.ID,
                                            Name = p.Name,
                                            Creator = p.Creator
                                                
                                        })
                                        .ToList();

            return (ISongDTO)_songMapper.FromDataModel(dto, users, playlists);
        }

        public List<Song> GetAllSongs(int playlistid)
        {
            var songDtos = _songRepo.GetSongList(playlistid).ToList();

            var users = _userRepo.GetAllUsers()
                         .Select(u => new User(_userRepo)
                         {
                             ID = u.ID,
                             Name = u.Name,
                             EmailAddress = u.EmailAddress,
                             PasswordHash = u.PasswordHash
                         })
                         .ToList();

            var playlists = _playlistRepo.GetAllPlaylists()
                                        .Select(p => new Playlist(_songRepo, _playlistRepo)
                                        {
                                            ID = p.ID,
                                            Name = p.Name,
                                            Creator = p.Creator
                                        })
                                        .ToList();

            return songDtos.Select(dto => _songMapper.FromDataModel(dto, users, playlists)).ToList();
        }
    }
}
