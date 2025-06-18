using System.Net.Http;
using Newtonsoft.Json;
using Interfaces;

namespace LogicLayer
{
    public class Playlist : IPlaylistDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime DateAdded { get; set; }
        public byte[] Photo { get; set; }
        public string Base64Photo { get; set; }
        public List<Song> PlaylistSongs { get; private set; } = new List<Song>();
        public IUserDTO Creator { get; set; }
        private IUserRepository userRepository;
        private ISongRepository songRepository;
        IPlaylistRepository playlistRepository;
        private readonly UserMapper userMapper;
        private readonly PlaylistMapper playlistMapper;
        private readonly SongMapper songMapper;
        
        public Playlist(ISongRepository songRepository, IPlaylistRepository playlistRepository, IUserRepository userRepository) 
        {
            this.songRepository = songRepository;
            this.playlistRepository = playlistRepository;
            this.userRepository = userRepository;
            playlistMapper = new PlaylistMapper(playlistRepository, songRepository, userRepository);
            songMapper = new SongMapper(songRepository, playlistRepository, userRepository);
            userMapper = new UserMapper(userRepository);
        }
        public void ChangePlaylistPicture(byte[] newPhoto)
        {
            Photo = newPhoto;
            playlistRepository.UpdatePlaylistPhoto(ID, newPhoto);
        }

        public void ChangePlaylistName(string newName)
        {
            if (newName == null || newName.Length == 0) 
            {
                throw new ArgumentException("Please fill in the space");
            }
            Name = newName;
            playlistRepository.UpdatePlaylistName(ID, newName);
        }

        public void AddSong(int songid)
        {
            if (songRepository.SongPlaylistCheck(ID, songid)) 
            {
                throw new InvalidOperationException("Cannot add duplicates of a song to a playlist");
            }
            songRepository.AddSongToPlaylist(ID, songid);

        }

        public void RemoveSong(int songid)
        {
            if (!songRepository.SongPlaylistCheck(ID, songid)) 
            {
                throw new InvalidOperationException("Cannot remove a song from a playlist that doesnt include that song");
            }
            songRepository.RemoveSongFromPlaylist(ID, songid);
        }

        public void SortSong(string field, string order)
        {
            playlistRepository.SortSongs(this.ID, field, order);
        }

		public void DeletePlaylist(int playlistID)
		{
			playlistRepository.DeletePlaylist(playlistID);
		}
        public Playlist GetSpecificPlaylist(int playlistid)
        {
            var playlistData = playlistRepository.GetPlaylistById(playlistid);

            if (playlistData != null)
            {
                var userDTOs = userRepository.GetAllUsers();
                var users = userDTOs
                    .Select(dto => userMapper.FromDataModel(dto))
                    .ToList();

                return playlistMapper.FromDataModel(playlistData, users);
            }

            return null;
        }
        public List<Song> LoadSongs(List<User> users, List<Playlist> playlists)
        {
            var songdata = songRepository.GetSongList(this.ID);
            List<Song> songs = new List<Song>();

            foreach (var item in songdata)
            {
                var song = songMapper.FromDataModel(item, users, playlists);
                songs.Add(song);
            }

            this.PlaylistSongs = songs;
            return songs;
        }

        private List<Song> ApplySortingOrShuffle(List<Song> songs, string field, string order, int? seed = null)
        {
            if (field?.ToLower() == "random" && seed.HasValue)
            {
                var rng = new Random(seed.Value);
                return songs.OrderBy(_ => rng.Next()).ToList();
            }

            bool ascending = order?.ToLower() == "asc";

            return field?.ToLower() switch
            {
                "name" => ascending ? songs.OrderBy(s => s.Name).ToList() : songs.OrderByDescending(s => s.Name).ToList(),
                "weight" => ascending ? songs.OrderBy(s => s.Weight).ToList() : songs.OrderByDescending(s => s.Weight).ToList(),
                "datereleased" => ascending ? songs.OrderBy(s => s.DateReleased).ToList() : songs.OrderByDescending(s => s.DateReleased).ToList(),
                "artistname" => ascending ? songs.OrderBy(s => s.Artist?.Name).ToList() : songs.OrderByDescending(s => s.Artist?.Name).ToList(),
                "albumname" => ascending ? songs.OrderBy(s => s.Album?.Name).ToList() : songs.OrderByDescending(s => s.Album?.Name).ToList(),
                _ => songs
            };
        }
        public void UpdatePlaylistList(string field = null, string order = null, int? shuffleSeed = null)
        {
            // Initialize mappers
            var userMapper = new UserMapper(userRepository);
            var playlistMapper = new PlaylistMapper(playlistRepository, songRepository, userRepository);
            var songMapper = new SongMapper(songRepository, playlistRepository, userRepository);  // Create an instance of SongMapper

            // Get the list of songs from the repository
            var dataModels = songRepository.GetSongList(this.ID);

            // Get distinct artist and album IDs from the songs
            var artistIds = dataModels.Select(dm => dm.Artist.ID).Distinct().ToList();
            var albumIds = dataModels.Select(dm => dm.Album.ID).Distinct().ToList();

            // Fetch user data models (artists) and playlist data models (albums)
            var userDataModels = userRepository.GetUsersByIds(artistIds);
            var playlistDataModels = playlistRepository.GetPlaylistsByIds(albumIds);

            // Map user data models to LogicLayer.User objects
            var users = userDataModels.Select(userMapper.FromDataModel).ToList();

            // Map user data models to IUserDTO for use in PlaylistMapper
            var userDTOs = users.Select(user => new User(userRepository)
            {
                ID = user.ID,
                Name = user.Name,
                EmailAddress = user.EmailAddress
            }).ToList();

            // Map playlist data models to LogicLayer.Playlist objects
            var playlists = playlistDataModels.Select(dm => playlistMapper.FromDataModel(dm, userDTOs)).ToList();

            // Map song data models to LogicLayer.Song objects
            var songs = dataModels.Select(dm => songMapper.FromDataModel(dm, users, playlists)).ToList();

            // Apply sorting or shuffle to the list of songs
            PlaylistSongs = ApplySortingOrShuffle(songs, field, order, shuffleSeed);
        }
    }
}
