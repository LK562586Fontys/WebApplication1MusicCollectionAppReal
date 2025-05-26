using DataAccessLayer;
using System.Net.Http;
using Newtonsoft.Json;

namespace LogicLayer
{
    public class Playlist
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime DateAdded { get; set; }
        public byte[] Photo { get; set; }
        public List<Song> PlaylistSongs { get; private set; } = new List<Song>();
        public User Creator { get; set; }
        private UserRepository userRepository = new UserRepository("Server=mssqlstud.fhict.local;Database=dbi562586_i562586;User Id=dbi562586_i562586;Password=Wpb3grVisq;TrustServerCertificate=True;");
        private SongRepository songRepository = new SongRepository("Server=mssqlstud.fhict.local;Database=dbi562586_i562586;User Id=dbi562586_i562586;Password=Wpb3grVisq;TrustServerCertificate=True;");
        PlaylistRepository playlistRepository = new PlaylistRepository("Server=mssqlstud.fhict.local;Database=dbi562586_i562586;User Id=dbi562586_i562586;Password=Wpb3grVisq;TrustServerCertificate=True;");
        public void ChangePlaylistPicture(byte[] newPhoto)
        {
            Photo = newPhoto;
            playlistRepository.UpdatePlaylistPhoto(ID, newPhoto);
        }

        public void ChangePlaylistName(string newName)
        {
            Name = newName;
            playlistRepository.UpdatePlaylistName(ID, newName);
        }

        public void AddSong(int songid)
        {
            songRepository.AddSongToPlaylist(ID, songid);

        }

        public void RemoveSong(int songid)
        {
            songRepository.RemoveSongFromPlaylist(ID, songid);
        }

        public void SortSong(string field, string order)
        {
            playlistRepository.SortSongs(this.ID, field, order);
        }

        public void ShuffleSong()
        {
            //Shuffle songs in this playlist
        }
		public void DeletePlaylist(int playlistID)
		{
			playlistRepository.DeletePlaylist(playlistID);
		}
		public List<Song> LoadSongs() 
        {
            var songdata = songRepository.GetSongList(this.ID);
            List<Song> songs = new List<Song>();
            foreach (var item in songdata)
            {
                songs.Add(new Song
                {
                    ID = item.ID,
                    Name = item.name,
                    Weight = item.weight,
                    DateReleased = item.dateReleased,
                });
            }
            this.PlaylistSongs = songs;
            return songs;

        }
        public void UpdatePlaylistList(string field = null, string order = null, int? shuffleSeed = null)
        {
            var dataModels = songRepository.GetSongList(this.ID);

            var artistIds = dataModels.Select(dm => dm.artistID).Distinct().ToList();
            var albumIds = dataModels.Select(dm => dm.albumID).Distinct().ToList();

            var userDataModels = userRepository.GetUsersByIds(artistIds);
            var playlistDataModels = playlistRepository.GetPlaylistsByIds(albumIds);

            var users = userDataModels.Select(UserMapper.FromDataModel).ToList();
            var playlists = playlistDataModels.Select(dm => PlaylistMapper.FromDataModel(dm, users)).ToList();

            var songs = dataModels.Select(dm => SongMapper.FromDataModel(dm, users, playlists)).ToList();

            PlaylistSongs = ApplySortingOrShuffle(songs, field, order, shuffleSeed);
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
    }
}
