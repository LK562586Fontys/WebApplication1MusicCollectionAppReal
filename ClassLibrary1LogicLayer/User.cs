using DataAccessLayer;

namespace LogicLayer
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
        public string ProfilePhoto { get; set; }
        public DateTime joinDate { get; set; }
        public List<Playlist> userPlaylist { get; set; } = new();
        private UserRepository rep = new UserRepository("Server=mssqlstud.fhict.local;Database=dbi562586_i562586;User Id=dbi562586_i562586;Password=Wpb3grVisq;TrustServerCertificate=True;");
        private PlaylistRepository db = new PlaylistRepository("Server=mssqlstud.fhict.local;Database=dbi562586_i562586;User Id=dbi562586_i562586;Password=Wpb3grVisq;TrustServerCertificate=True;");

        public User Register(string name, string emailAddress, string passwordHash)
        {
            return Register(name, emailAddress, passwordHash);
        }

        public void ChangeUsername(string newName)
        {
            Name = newName;
            rep.UpdateUsername(ID, newName);
        }

        public void ChangeEmailAddress(string newEmail)
        {
            EmailAddress = newEmail;
            rep.UpdateEmail(ID, newEmail);
        }

        public void ChangePassword(string newPassword)
        {
            PasswordHash = newPassword;
            rep.UpdatePassword(ID, newPassword);
        }

        private void ChangeProfilePhoto(string newPhoto)
        {
            //change your Profile Photo
        }

        public void AddPlaylist() //Must
        {
            string generatedName = $"Playlist #{userPlaylist.Count + 1}";
            DateTime now = DateTime.Now;
            var newPlaylist = new Playlist
            {
                Name = generatedName,
                DateAdded = now,
                Creator = this
            };
            userPlaylist.Add(newPlaylist);
            PlaylistRepository db = new PlaylistRepository("Server=mssqlstud.fhict.local;Database=dbi562586_i562586;User Id=dbi562586_i562586;Password=Wpb3grVisq;TrustServerCertificate=True;");
            db.InsertPlaylist(generatedName, now, this.ID);
        }

        private void DeletePlaylist(Playlist oldPlaylist)
        {
            //Delete a playlist
        }

        private void DeleteAccount()
        {
            //Delete your account
        }
        public List<Playlist> LoadPlaylists()
        {
            var dataModels = db.LoadPlaylists(this.ID);
            List<Playlist> playlists = new List<Playlist>();
            foreach (var item in dataModels)
            {
                playlists.Add(new Playlist
                {
                    ID = item.ID,
                    Name = item.Name,
                    DateAdded = item.DateAdded,
                });
            }
            this.userPlaylist = playlists;
            return playlists;
        }
    }
}
