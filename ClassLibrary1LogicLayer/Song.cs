using Interfaces;

namespace LogicLayer
{
    public class Song
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public DateTime DateReleased { get; private set; }
        public int Weight { get; private set; }
        public User Artist { get; private set; }
        public Playlist Album { get; private set; }
        public ISongRepository songRepository;
        public IPlaylistRepository playlistRepository;
        public IUserRepository userRepository;
		private UserMapper userMapper;
		private PlaylistMapper playlistMapper;
		private SongMapper songMapper;

		public Song(int id, string name, DateTime dateReleased, int weight, User artist, Playlist album) 
        {
            ID = id;
            Name = name;
            DateReleased = dateReleased;
            Weight = weight;
            Artist = artist;
            Album = album;
		}
		public void InitialiseRepositories(IUserRepository userRepo, IPlaylistRepository playlistRepo, ISongRepository songRepo)
		{
			this.userRepository = userRepo;
			this.playlistRepository = playlistRepo;
			this.songRepository = songRepo;

			playlistMapper = new PlaylistMapper();
			userMapper = new UserMapper();
			songMapper = new SongMapper();
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
				var users = userRepository.GetAllUsers()
				.Select(u => userMapper.FromDataModel(u))
				.ToList();

				var playlists = playlistRepository.GetAllPlaylists()
				.Select(p => playlistMapper.FromDataModel(p, users))
				.ToList();

				return songMapper.FromDataModel(data, users, playlists);

			}

            return null;
        }
        public List<Song> SearchSongs(string searchTerm)
        {
            var userMapper = new UserMapper();
            var playlistMapper = new PlaylistMapper();
            var songMapper = new SongMapper();

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
