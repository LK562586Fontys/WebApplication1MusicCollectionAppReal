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

        private void PlaySong()
        {
            //Play or replace a song
        }

        private void ChangeSongWeight(int weight)
        {
            //change song weight from -10 to +10
            //if (weight < -10 || weight > 10) { }
        }

    }
}
