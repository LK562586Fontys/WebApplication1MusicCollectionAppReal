using Interfaces;

namespace LogicLayer
{
    public class Song : ISongDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime DateReleased { get; set; }
        public int Weight { get; set; }
        public List<Genre> GenreSong { get; set; }
        public List<Playlist> PlaylistSong { get; set; }
        public IUserDTO Artist { get; set; }
        public IPlaylistDTO Album { get; set; }
        public Genre Genre { get; set; }
        public ISongRepository songRepository;
        public IPlaylistRepository playlistRepository;
        public IUserRepository userRepository;

        public Song(ISongRepository songRepository, IUserRepository userRepository, IPlaylistRepository playlistRepository) 
        {
            this.songRepository = songRepository;
            this.userRepository = userRepository;
            this.playlistRepository = playlistRepository;
        }
        private void PlaySong()
        {
            //Play or replace a song
        }

        public void ChangeSongWeight(int songID, int weight)
        {
            int newWeight = Weight + weight;
            if (newWeight > 10 || newWeight < -10) 
            {
                throw new ArgumentException("Song weight above/below the limit");
            }
            Weight = Weight + weight;
            songRepository.ChangeSongWeight(songID, weight);
        }
        public Song GetSpecificSong(int songId)
        {
            var data = songRepository.GetSpecificSong(songId);

            if (data != null)
            {
                return new Song(songRepository, userRepository, playlistRepository)
                {
                    ID = data.ID,
                    Name = data.Name,
                    Weight = data.Weight,
                    DateReleased = data.DateReleased
                };
            }

            return null;
        }
        public List<Song> SearchSongs(string searchTerm)
        {
            var userMapper = new UserMapper(userRepository);
            var playlistMapper = new PlaylistMapper(playlistRepository, songRepository, userRepository);
            var songMapper = new SongMapper(songRepository, playlistRepository, userRepository);

            // Get all users and map them
            var userDataModels = userRepository.GetAllUsers();
            var users = userDataModels
                .Select(udm => userMapper.FromDataModel(udm))
                .ToList();

            // Map LogicLayer.User to IUserDTO manually (if User doesn't implement IUserDTO)
            var userDTOs = users.Select(user => new User(userRepository)
            {
                ID = user.ID,
                Name = user.Name,
                EmailAddress = user.EmailAddress
            }).ToList();

            // Get all playlists and map them (requires users for creator)
            var playlistDataModels = playlistRepository.GetAllPlaylists();
            var playlists = playlistDataModels
                .Select(pm => playlistMapper.FromDataModel(pm, userDTOs))  // Pass userDTOs here
                .ToList();

            // Get songs filtered by search term
            var songDataModels = songRepository.SearchSongs(searchTerm);

            // Map song DTOs to song domain models
            var allSongs = songDataModels
                .Select(dm => songMapper.FromDataModel(dm, users, playlists))  // Call on instance of songMapper
                .ToList();

            return allSongs;
        }
    }
}
