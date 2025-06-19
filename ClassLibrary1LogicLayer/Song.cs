using Interfaces;

namespace LogicLayer
{
    public class Song
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime DateReleased { get; set; }
        public int Weight { get; set; }
        public IUserDTO Artist { get; set; }
        public IPlaylistDTO Album { get; set; }
        public ISongRepository songRepository;
        public IPlaylistRepository playlistRepository;
        public IUserRepository userRepository;

        public Song(int id, string name, DateTime dateReleased, int weight, IUserDTO artist, IPlaylistDTO album) 
        {
            ID = id;
            Name = name;
            DateReleased = dateReleased;
            Weight = weight;
            Artist = artist;
            Album = album;
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
                return new Song(
                    ID = data.ID,
                    Name = data.Name,
					DateReleased = data.DateReleased,
					Weight = data.Weight,
                    Artist = data.Artist,
                    Album = data.Album
                    );
            }

            return null;
        }
        public List<Song> SearchSongs(string searchTerm)
        {
            var userMapper = new UserMapper(userRepository);
            var playlistMapper = new PlaylistMapper(playlistRepository, songRepository, userRepository);
            var songMapper = new SongMapper(songRepository, playlistRepository, userRepository);

            var userDataModels = userRepository.GetAllUsers();
            var users = userDataModels
                .Select(udm => userMapper.FromDataModel(udm))
                .ToList();

			var userDTOs = users.Select(user => new User(
			        id: user.ID,
			        name: user.Name,
			        emailAddress: user.EmailAddress,
			        passwordHash: user.PasswordHash,
			        joinDate: user.JoinDate,
			        profilePhoto: user.ProfilePhoto)).ToList();

			var playlistDataModels = playlistRepository.GetAllPlaylists();
            var playlists = playlistDataModels
                .Select(pm => playlistMapper.FromDataModel(pm, userDTOs))
                .ToList();

            var songDataModels = songRepository.SearchSongs(searchTerm);

            var allSongs = songDataModels
                .Select(dm => songMapper.FromDataModel(dm, users, playlists))
                .ToList();

            return allSongs;
        }
    }
}
