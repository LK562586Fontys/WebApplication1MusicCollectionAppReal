using LogicLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTesting
{
    [TestClass]
    public class PlaylistIntegrationTests
    {
        private Playlist playlistObject;
        [TestInitialize]
        public void Setup()
        {
            playlistObject = new Playlist { ID = 1015 };
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
            Assert.IsNull(playlistfromdatabase.Name);
            Assert.IsNull(playlistfromdatabase.ID);
        }
        [TestMethod]
        public void IntegrationTestAddSong()
        {
            //Arrange
            Song song = new Song { ID = 8 };
            int songid = song.ID;
            playlistObject.LoadSongs();
            Assert.IsFalse(playlistObject.PlaylistSongs.Any(s => s.ID == song.ID));
            //Act
            playlistObject.AddSong(songid);
            //Assert
            playlistObject.LoadSongs();
            Assert.IsTrue(playlistObject.PlaylistSongs.Any(s => s.ID == songid));
        }
        [TestMethod]
        public void IntegrationTestRemoveSong()
        {
            //Arrange
            Song song = new Song { ID = 8 };
            int songid = song.ID;
            playlistObject.LoadSongs();
            Assert.IsTrue(playlistObject.PlaylistSongs.Any(s => s.ID == songid));
            //Act
            playlistObject.RemoveSong(songid);
            //Assert
            playlistObject.LoadSongs();
            Assert.IsFalse(playlistObject.PlaylistSongs.Any(s => s.ID == songid));
        }
    }
}
