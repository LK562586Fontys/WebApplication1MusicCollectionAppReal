using DataAccessLayer;

namespace LogicLayer
{
    public class Song
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime DateReleased { get; set; }
        public int Duration { get; set; }
        public int Weight { get; set; }
        public List<Genre> GenreSong { get; set; }
        public List<Playlist> PlaylistSong { get; set; }
        public User Artist { get; set; }
        public Playlist Album { get; set; }
        public Genre Genre { get; set; }
        public SongRepository songRepository = new SongRepository("Server=mssqlstud.fhict.local;Database=dbi562586_i562586;User Id=dbi562586_i562586;Password=Wpb3grVisq;TrustServerCertificate=True;");

        private void PlaySong()
        {
            //Play or replace a song
        }

        public void ChangeSongWeight(int songID, int weight)
        {
            Weight = weight;
            songRepository.ChangeSongWeight(songID, weight);
        }
        public void GetAllSongs() 
        {
            songRepository.GetAllSongs();
        }
        public void GetSpecificSong(int songId)
        {
            songRepository.GetSpecificSong(songId);
        }
    }
}
