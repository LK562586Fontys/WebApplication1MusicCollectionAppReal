namespace WebApplication1MusicCollectionAppReal
{
    public class Song
    {
        private int ID { get; set; }
        private string name { get; set; }
        private DateTime dateReleased { get; set; }
        private int duration { get; set; }
        private int weight { get; set; }
        private List<Genre> genreSong { get; set; }
        private List<Playlist> playlistSong { get; set; }
        private User artist { get; set; }
        private Playlist album { get; set; }
        private Genre genre { get; set; }
        private void PlaySong() 
        {
            //Play or replace a song
        }
        private void ChangeSongWeight(int weight) 
        {
            //change song weight from -10 to +10
            if (weight < -10 || weight > 10) { }
        }

    }
}
