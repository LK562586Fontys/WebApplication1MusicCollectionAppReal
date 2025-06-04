using DataAccessLayer;

namespace LogicLayer
{
    public class Song
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime DateReleased { get; set; }
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
            if (Weight > 10 || Weight < -10) { return; }
            songRepository.ChangeSongWeight(songID, weight);
        }
        public Song GetSpecificSong(int songId)
        {
            var data = songRepository.GetSpecificSong(songId);

            if (data != null)
            {
                return new Song
                {
                    ID = data.ID,
                    Name = data.name,
                    Weight = data.weight,
                    DateReleased = data.dateReleased
                };
            }

            return null;
        }
        public List<Song> SearchSongs(string searchTerm)
        {
            var songRepo = new SongRepository("Server=mssqlstud.fhict.local;Database=dbi562586_i562586;User Id=dbi562586_i562586;Password=Wpb3grVisq;TrustServerCertificate=True;");
            var userRepo = new UserRepository("Server=mssqlstud.fhict.local;Database=dbi562586_i562586;User Id=dbi562586_i562586;Password=Wpb3grVisq;TrustServerCertificate=True;");
            var playlistRepo = new PlaylistRepository("Server=mssqlstud.fhict.local;Database=dbi562586_i562586;User Id=dbi562586_i562586;Password=Wpb3grVisq;TrustServerCertificate=True;");

            var userDataModels = userRepo.GetAllUsers();
            var users = userDataModels
                .Select(udm => UserMapper.FromDataModel(udm))
                .ToList();

            // Get all playlists and map them (requires users for creator)
            var playlistDataModels = playlistRepo.GetAllPlaylists();
            var playlists = playlistDataModels
                .Select(pm => PlaylistMapper.FromDataModel(pm, users))
                .ToList();

            // Get filtered songs based on search term
            var songDataModels = songRepo.SearchSongs(searchTerm);

            // Map songs to logic layer
            var allSongs = songDataModels
                .Select(dm => SongMapper.FromDataModel(dm, users, playlists))
                .ToList();

            return allSongs;
        }
    }
}
