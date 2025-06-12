using LogicLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccessLayer;

namespace UnitTesting
{
    [TestClass]
    public class PlaylistIntegrationTests
    {
        private Playlist playlistObject;
        private PlaylistService _playlistService;
        private SongService _songService;
        private SongRepository _songRepository;
        private UserRepository _userRepository;
        private PlaylistRepository _playlistRepository;

        private Song playlistSongObject;
        [TestInitialize]
        public void Setup()
        {
            var playlistRepo = new PlaylistRepository("Server=mssqlstud.fhict.local;Database=dbi562586_i562586;User Id=dbi562586_i562586;Password=Wpb3grVisq;TrustServerCertificate=True;");
            var userRepo = new UserRepository("Server=mssqlstud.fhict.local;Database=dbi562586_i562586;User Id=dbi562586_i562586;Password=Wpb3grVisq;TrustServerCertificate=True;");
            var songRepo = new SongRepository("Server=mssqlstud.fhict.local;Database=dbi562586_i562586;User Id=dbi562586_i562586;Password=Wpb3grVisq;TrustServerCertificate=True;");

            _playlistService = new PlaylistService(playlistRepo, userRepo, songRepo);
            _songService = new SongService(songRepo, userRepo, playlistRepo);

            playlistObject = (Playlist)_playlistService.GetPlaylistById(1004)!;
            playlistSongObject = (Song)_songService.GetSongById(10)!;
        }
        [TestMethod]
        public void IntegrationTestChangePlaylistPicture()
        {
            //Arrange
            byte[] newpicture = { 1, 2, 3, 4 };
            //Act
            playlistObject.ChangePlaylistPicture(newpicture);
            //Assert
            Assert.AreEqual(newpicture, playlistObject.Photo);
            var playlistfromdatabase = playlistObject.GetSpecificPlaylist(playlistObject.ID);
            CollectionAssert.AreEqual(newpicture, playlistfromdatabase.Photo);
        }
        [TestMethod]
        public void IntegrationTestChangePlaylistName()
        {
            //Arrange
            string newname = "MyNewPlaylistname";
            //Act
            playlistObject.ChangePlaylistName(newname);
            //Assert
            var playlistfromdatabase = playlistObject.GetSpecificPlaylist(playlistObject.ID);
            Assert.AreEqual(newname, playlistObject.Name);
        }
        [TestMethod]
        public void IntegrationTestDeletePlaylist()
        {
            //Arrange
            int skadoosh = playlistObject.ID;
            //Act
            playlistObject.DeletePlaylist(skadoosh);
            //Assert
            var playlistfromdatabase = playlistObject.GetSpecificPlaylist(playlistObject.ID);
            Assert.IsNull(playlistfromdatabase);
        }
        [TestMethod]
        public void IntegrationTestAddSong()
        {
            //Arrange
            int songid = playlistSongObject.ID;
            playlistObject.LoadSongs(new List<User>(), new List<Playlist>());
            Assert.IsFalse(playlistObject.PlaylistSongs.Any(s => s.ID == songid));
            //Act
            playlistObject.AddSong(songid);
            //Assert
            playlistObject.LoadSongs(new List<User>(), new List<Playlist>());
            Assert.IsTrue(playlistObject.PlaylistSongs.Any(s => s.ID == songid));
        }
        [TestMethod]
        public void IntegrationTestRemoveSong()
        {
            //Arrange
            int songid = playlistSongObject.ID;
            playlistObject.LoadSongs(new List<User>(), new List<Playlist>());
            Assert.IsTrue(playlistObject.PlaylistSongs.Any(s => s.ID == songid));
            //Act
            playlistObject.RemoveSong(songid);
            //Assert
            playlistObject.LoadSongs(new List<User>(), new List<Playlist>());
            Assert.IsFalse(playlistObject.PlaylistSongs.Any(s => s.ID == songid));
        }
        [TestMethod]
        public void AddSongExceptionHandlingIntegration() 
        {
            // Arrange
            int songid = playlistSongObject.ID;
            playlistObject.LoadSongs(new List<User>(), new List<Playlist>());
            Assert.IsTrue(playlistObject.PlaylistSongs.Any(s => s.ID == songid));

            //Act & Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(() => playlistObject.AddSong(songid));
            Assert.AreEqual("Cannot add duplicates of a song to a playlist", ex.Message);
        }
        [TestMethod]
        public void RemoveSongExceptionHandlingIntegration()
        {
            // Arrange
            int songid = playlistSongObject.ID;
            playlistObject.LoadSongs(new List<User>(), new List<Playlist>());
            Assert.IsFalse(playlistObject.PlaylistSongs.Any(s => s.ID == songid));

            //Act & Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(() => playlistObject.RemoveSong(songid));
            Assert.AreEqual("Cannot remove a song from a playlist that doesnt include that song", ex.Message); 
        }
    }
}
