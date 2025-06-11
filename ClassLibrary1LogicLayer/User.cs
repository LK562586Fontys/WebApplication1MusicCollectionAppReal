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
        private readonly IUserRepository userRepository;
        private IPlaylistRepository playlistRepository;
        private ISongRepository songRepository;
        private readonly UserMapper userMapper;
        private readonly PlaylistMapper playlistMapper;

        public User(IUserRepository userRepositorys)
        {
            this.userRepository = userRepositorys;
            userMapper = new UserMapper(userRepositorys);
        }

        public void ChangeUsername(string newName)
        {
            Name = newName;
            userRepository.UpdateUsername(ID, newName);
        }

        public void ChangeEmailAddress(string newEmail)
        {
            EmailAddress = newEmail;
            userRepository.UpdateEmail(ID, newEmail);
        }

        public void ChangePassword(string newPassword)
        {
            PasswordHash = newPassword;
            userRepository.UpdatePassword(ID, newPassword);
        }

        public void ChangeProfilePhoto(byte[] newPhoto)
        {
            ProfilePhoto = newPhoto;
            userRepository.UpdateProfilePhoto(ID, newPhoto);
        }

        public void AddPlaylist(DateTime CurrentDate, IPlaylistRepository playlistRepo)
        {
            string generatedName = $"Playlist #{userPlaylist.Count + 1}";
            var newPlaylist = new Playlist(songRepository, playlistRepository, userRepository)
            {
                Name = generatedName,
                DateAdded = CurrentDate,
                Creator = this,
            };
            userPlaylist.Add(newPlaylist);
            playlistRepo.InsertPlaylist(generatedName, CurrentDate, this.ID);
        }

        public void DeleteAccount(int userID)
        {
            userRepository.DeleteAccount(this.ID);
            
        }

        public List<Playlist> LoadPlaylists(IPlaylistRepository playlistRepo, ISongRepository songRepo, string playlistOrderCookie = null)
        {
            var dataModels = playlistRepo.LoadPlaylists(this.ID);
            List<Playlist> playlists = dataModels.Select(item => new Playlist(songRepo, playlistRepo, userRepository)
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

                var missing = dataModels.Where(dm => !orderedIds.Contains(dm.ID)).Select(item => new Playlist(songRepository, playlistRepository, userRepository)
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
            var userData = userRepository.GetSpecificUser(userid);

            if (userData != null)
            {
                return userMapper.FromDataModel(userData);
            }

            return null;
        }
	}
} 