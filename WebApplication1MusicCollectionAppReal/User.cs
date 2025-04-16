namespace WebApplication1MusicCollectionAppReal
{
    public class User
    {
        private int ID { get; set; }
        private string name { get; set; }
        private string emailAddress { get; set; }
        private string passwordHash { get; set; }
        private string phoneNumber { get; set; }
        private string profilePhoto { get; set; }
        private DateTime joinDate { get; set; }
        private List<Playlist> userPlaylist { get; set; }
        public User Register(string name, string emailAddress, string passwordHash) 
        {
            return this.Register(name, emailAddress, passwordHash);
        }
        private void ChangeUsername(string newName)
        {
            //change your username
        }
        private void ChangeEmailAddress(string newEmail)
        {
            //change your Email Address
        }
        private void ChangePassword(string newPassword)
        {
            //change your Password
        }
        private void ChangeProfilePhoto(string newPhoto)
        {
            //change your Profile Photo
        }
        private void AddPlaylist()
        {
            //Add a playlist
            Playlist newPlaylist = new Playlist();
        }
        private void DeletePlaylist(Playlist oldPlaylist)
        {
            //Delete a playlist
        }
        private void DeleteAccount()
        {
            //Delete your account
        }
    }
}
