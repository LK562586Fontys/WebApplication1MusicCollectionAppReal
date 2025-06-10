using Interfaces;
using LogicLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace UnitTesting
{
    [TestClass]
    public class SongTests
    {
        private SongService _songService;
        private Song songObject;
        private List<Song> _playlistSongs;
        private List<Song> _searchedSongs;

        [TestInitialize]
        public void Setup()
        {
            var songRepoMock = new SongRepositoryMock();
            var userRepoMock = new UserRepositoryMock();
            var playlistRepoMock = new PlaylistRepositoryMock();

            _songService = new SongService(songRepoMock, userRepoMock, playlistRepoMock);

            songObject = (Song)_songService.GetSongById(1)!;
            _playlistSongs = _songService.GetAllSongs(1);
        }
        [TestMethod]
        public void TestChangeSongWeight()
        {
            //Arrange
            int Weight = 9;
            //Act
            songObject.ChangeSongWeight(songObject.ID, Weight);
            //Assert
            Assert.AreEqual(Weight, songObject.Weight);
        }
    }
}
