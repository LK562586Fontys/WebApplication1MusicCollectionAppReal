﻿using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class PlaylistFactory
    {
        private readonly IPlaylistRepository _playlistRepo;
        private readonly IUserRepository _userRepo;
        private readonly ISongRepository _songRepo;
        private readonly PlaylistMapper _playlistMapper;

        public PlaylistFactory(IPlaylistRepository playlistRepo, IUserRepository userRepo, ISongRepository songRepo)
        {
            _playlistRepo = playlistRepo;
            _userRepo = userRepo;
            _songRepo = songRepo;
            _playlistMapper = new PlaylistMapper();
        }

        public Playlist? GetPlaylistById(int id)
        {
            var dto = _playlistRepo.GetPlaylistById(id);
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

			var playlist = _playlistMapper.FromDataModel(dto, users);

            playlist.InitialiseRepositories(_userRepo, _playlistRepo, _songRepo);

            return playlist;
        }

    }
}
