using LogicLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace UnitTesting
{
    [TestClass]
    public class PlaylistTests
    {
        private PlaylistFactory _playlistFactory;
        private SongFactory _songFactory;

        private Playlist playlistObject;
        private Song songObject;

        [TestInitialize]
        public void Setup()
        {
            // Mock Repositories
            var playlistRepoMock = new PlaylistRepositoryMock();
            var userRepoMock = new UserRepositoryMock();
            var songRepoMock = new SongRepositoryMock();

            // Services
            _playlistFactory = new PlaylistFactory(playlistRepoMock, userRepoMock, songRepoMock);
            _songFactory = new SongFactory(songRepoMock, userRepoMock, playlistRepoMock);

            // Get a specific playlist (ensure ID exists in your mock)
            playlistObject = (Playlist)_playlistFactory.GetPlaylistById(1)!;

            // Get a single song from that playlist
            songObject = _songFactory.GetAllSongs(playlistObject.ID).First();
        }
        [TestMethod]
        public void TestChangePlaylistPicture() 
        {
            //Arrange
            byte[] newpicture = { 1, 2, 3, 4 };
            //Act
            playlistObject.ChangePlaylistPhoto(newpicture);
            //Assert
            Assert.AreEqual(newpicture, playlistObject.Photo);
        }
        [TestMethod]
        public void TestChangePlaylistName()
        {
            //Arrange
            string newname = "MyNewPlaylistname";
            //Act
            playlistObject.ChangePlaylistName(newname);
            //Assert
            Assert.AreEqual(newname, playlistObject.Name);
        }
        [TestMethod]
        public void TestDeletePlaylist()
        {
            //Arrange
            int skadoosh = playlistObject.ID;
            //Act
            playlistObject.DeletePlaylist(skadoosh);
            //Assert
            Assert.IsNull(playlistObject.Name);
        }
        [TestMethod]
        public void TestAddSong() 
        {
            //Arrange
            int songid = songObject.ID;
            //Act
            playlistObject.AddSong(songid);
            //Assert
            Assert.IsTrue(playlistObject.PlaylistSongs.Any(s => s.ID == songid));
        }
        [TestMethod]
        public void TestRemoveSong()
        {
            //Arrange
            int songid = songObject.ID;
            //Act
            playlistObject.RemoveSong(songid);
            //Assert
            Assert.IsFalse(playlistObject.PlaylistSongs.Any(s => s.ID == songid));
        }
        [TestMethod]
        public void ChangePlaylistNameExceptionHandling()
        {
            // Arrange
            string newplaylistname = null;

            //Act & Assert
            var ex = Assert.ThrowsException<ArgumentException>(() => playlistObject.ChangePlaylistName(newplaylistname));
            Assert.AreEqual("Please fill in the space", ex.Message);
        }
    }
}
