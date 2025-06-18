using LogicLayer;

namespace WebApplication1MusicCollectionAppReal.Pages.ViewModels
{
    public class SongViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public DateTime DateReleased { get; set; }
        public User Artist { get; set; }
        public Playlist Album { get; set; }
    }
}
