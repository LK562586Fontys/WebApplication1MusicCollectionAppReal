using Interfaces;

namespace LogicLayer
{
    public class User : IUserDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
        public byte[] ProfilePhoto { get; set; }
        public DateTime joinDate { get; set; }
        public List<Playlist> userPlaylist { get; set; } = new();
        private readonly IUserRepository userRepositorys;
        private IPlaylistRepository playlistRepository;
        private ISongRepository songRepository;
        private readonly UserMapper userMapper;
        private readonly PlaylistMapper playlistMapper;

        public User(IUserRepository userRepositorys)
        {
            this.userRepositorys = userRepositorys;
        }

        public void ChangeUsername(string newName)
        {
            Name = newName;
            userRepositorys.UpdateUsername(ID, newName);
        }

        public void ChangeEmailAddress(string newEmail)
        {
            EmailAddress = newEmail;
            userRepositorys.UpdateEmail(ID, newEmail);
        }

        public void ChangePassword(string newPassword)
        {
            PasswordHash = newPassword;
            userRepositorys.UpdatePassword(ID, newPassword);
        }

        public void ChangeProfilePhoto(byte[] newPhoto)
        {
            ProfilePhoto = newPhoto;
            userRepositorys.UpdateProfilePhoto(ID, newPhoto);
        }

        public void AddPlaylist(DateTime CurrentDate)
        {
            string generatedName = $"Playlist #{userPlaylist.Count + 1}";
            var newPlaylist = new Playlist(songRepository, playlistRepository)
            {
                Name = generatedName,
                DateAdded = CurrentDate,
                Creator = this,
            };
            userPlaylist.Add(newPlaylist);
            playlistRepository.InsertPlaylist(generatedName, CurrentDate, this.ID);
        }

        public void DeleteAccount(int userID)
        {
            userRepositorys.DeleteAccount(this.ID);
            
        }

        public List<Playlist> LoadPlaylists(string playlistOrderCookie = null)
        {
            var dataModels = playlistRepository.LoadPlaylists(this.ID);
            List<Playlist> playlists = dataModels.Select(item => new Playlist(songRepository, playlistRepository)
            {
                ID = item.ID,
                Name = item.Name,
                DateAdded = item.DateAdded,
                Photo = item.Photo,
                Creator = this
            }).ToList();

            if (!string.IsNullOrEmpty(playlistOrderCookie))
            {
                var idStrings = playlistOrderCookie.Split(',');
                var orderedIds = idStrings.Select(idStr => int.TryParse(idStr, out int id) ? id : -1).Where(id => id != -1).ToList();

                playlists = orderedIds.Select(id => playlists.FirstOrDefault(p => p.ID == id))
                    .Where(p => p != null).ToList();

                var missing = dataModels.Where(dm => !orderedIds.Contains(dm.ID)).Select(item => new Playlist(songRepository, playlistRepository)
                {
                    ID = item.ID,
                    Name = item.Name,
                    DateAdded = item.DateAdded,
                    Photo = item.Photo,
                    Creator = this
                });

                playlists.AddRange(missing);
            }

            this.userPlaylist = playlists;
            return playlists;
        }

        public User GetSpecificUser(int userid)
        {
            var userData = userRepositorys.GetSpecificUser(userid);

            if (userData != null)
            {
                return userMapper.FromDataModel(userData);
            }

            return null;
        }
        public async Task<int?> VerifyLoginAndReturnUserId(string email, string password)
		{
			return await userRepositorys.VerifyLoginAndReturnUserId(email, password);
		}
	}
} 