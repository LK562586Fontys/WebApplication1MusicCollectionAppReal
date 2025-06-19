using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
	public class UserFactory
	{
		private readonly IUserRepository userRepo;
		private readonly IPlaylistRepository playlistRepo;
		private readonly ISongRepository songRepo;
		private readonly UserMapper mapper;

		public UserFactory(
			IUserRepository userRepo,
			IPlaylistRepository playlistRepo,
			ISongRepository songRepo)
		{
			this.userRepo = userRepo;
			this.playlistRepo = playlistRepo;
			this.songRepo = songRepo;
			this.mapper = new UserMapper();
		}

		public User? GetUserById(int id)
		{
			var dto = userRepo.GetSpecificUser(id);
			if (dto == null) return null;

			var user = mapper.FromDataModel(dto);

			user.InitialiseRepositories(userRepo, playlistRepo, songRepo);

			return user;
		}

		public async Task<int?> VerifyLoginAndReturnUserId(string email, string password)
		{
			return await userRepo.VerifyLoginAndReturnUserId(email, password);
		}
	}
}
