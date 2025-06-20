using Interfaces;
using LogicLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccessLayer;

namespace UnitTesting
{
    [TestClass]
    public class SongIntegrationTests
    {
        private SongFactory _songFactory;
        private Song songObject;
        [TestInitialize]
        public void Setup()
        {
            var songRepo = new SongRepository("Server=mssqlstud.fhict.local;Database=dbi562586_i562586;User Id=dbi562586_i562586;Password=Wpb3grVisq;TrustServerCertificate=True;");
            var userRepo = new UserRepository("Server=mssqlstud.fhict.local;Database=dbi562586_i562586;User Id=dbi562586_i562586;Password=Wpb3grVisq;TrustServerCertificate=True;");
            var playlistRepo = new PlaylistRepository("Server=mssqlstud.fhict.local;Database=dbi562586_i562586;User Id=dbi562586_i562586;Password=Wpb3grVisq;TrustServerCertificate=True;");

            _songFactory = new SongFactory(songRepo, userRepo, playlistRepo);

            songObject = _songFactory.GetSongById(1)!;
        }
        [TestMethod]
        public void IntegrationTestChangeSongWeight()
        {
            //Arrange
            int originalWeight = songObject.GetSpecificSong(songObject.ID).Weight;
            int Weight = 1;
            //Act
            songObject.ChangeSongWeight(songObject.ID, Weight);
            //Assert
            var songfromdatabase = songObject.GetSpecificSong(songObject.ID);
            Assert.AreEqual(originalWeight + Weight, songObject.GetSpecificSong(songObject.ID).Weight);
        }
    }
}
