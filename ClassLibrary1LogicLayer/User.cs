using DataAccessLayer;

namespace LogicLayer
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
        public byte[] ProfilePhoto { get; set; }
        public DateTime joinDate { get; set; }
        public List<Playlist> userPlaylist { get; set; } = new();
        private UserRepository userRepository = new UserRepository("Server=mssqlstud.fhict.local;Database=dbi562586_i562586;User Id=dbi562586_i562586;Password=Wpb3grVisq;TrustServerCertificate=True;");
        private PlaylistRepository playlistRepository = new PlaylistRepository("Server=mssqlstud.fhict.local;Database=dbi562586_i562586;User Id=dbi562586_i562586;Password=Wpb3grVisq;TrustServerCertificate=True;");

        public User() 
        {
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

        public void AddPlaylist(DateTime CurrentDate)
        {
            string generatedName = $"Playlist #{userPlaylist.Count + 1}";
            var newPlaylist = new Playlist
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
            userRepository.DeleteAccount(this.ID);
            
        }

        public List<Playlist> LoadPlaylists(string playlistOrderCookie = null)
        {
            var dataModels = playlistRepository.LoadPlaylists(this.ID);
            List<Playlist> playlists = dataModels.Select(item => new Playlist
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

                var missing = dataModels.Where(dm => !orderedIds.Contains(dm.ID)).Select(item => new Playlist
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
            var users = userRepository.GetSpecificUser(userid);

            if (users.Count > 0)
            {
                var userData = users[0];
                return new User
                {
                    ID = userData.ID,
                    Name = userData.userName,
                    EmailAddress = userData.email,
                    joinDate = userData.joinDate,
                    ProfilePhoto = userData.picture,
                    PasswordHash = userData.password,
                };
            }

            return null;
        }
        public async Task<int?> VerifyLoginAndReturnUserId(string email, string password)
		{
			return await userRepository.VerifyLoginAndReturnUserId(email, password);
		}
	}
} 