using LogicLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace UnitTesting
{
    [TestClass]
    public class PlaylistTests
    {
        private Playlist playlistObject;
        [TestInitialize]
        public void Setup()
        {
            playlistObject = new Playlist { ID = 1015 };
        }
        [TestMethod]
        public void TestChangePlaylistPicture() 
        {
            //Arrange
            byte[] newpicture = { 1, 2, 3, 4 };
            //Act
            playlistObject.ChangePlaylistPicture(newpicture);
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
            Song song = new Song { ID = 8 };
            int songid = song.ID;
            //Act
            playlistObject.AddSong(songid);
            //Assert
            Assert.IsTrue(playlistObject.PlaylistSongs.Any(s => s.ID == songid));
        }
        [TestMethod]
        public void TestRemoveSong()
        {
            //Arrange
            Song song = new Song { ID = 8 };
            int songid = song.ID;
            //Act
            playlistObject.RemoveSong(songid);
            //Assert
            Assert.IsFalse(playlistObject.PlaylistSongs.Any(s => s.ID == songid));
        }
    }
}
