using LogicLayer;

namespace WebApplication1MusicCollectionAppReal.Pages.ViewModels
{
    public class AccountViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
        public byte[] ProfilePhoto { get; set; }
        public DateTime joinDate { get; set; }
        public string ProfileBase64Photo { get; set; }
        public List<Playlist> Playlists { get; set; } = new();
    }
}
