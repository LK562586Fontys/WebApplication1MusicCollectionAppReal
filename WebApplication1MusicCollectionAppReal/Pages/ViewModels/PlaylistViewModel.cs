namespace WebApplication1MusicCollectionAppReal.Pages.ViewModels
{
    public class PlaylistViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime DateAdded { get; set; }
        public byte[] Photo { get; set; }
        public string Base64Photo { get; set; }
    }
}
