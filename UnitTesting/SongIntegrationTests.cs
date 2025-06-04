using LogicLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTesting
{
    [TestClass]
    public class SongIntegrationTests
    {
        private Song songObject;
        [TestInitialize]
        public void Setup()
        {
            songObject = new Song { ID = 5 };
        }
        [TestMethod]
        public void IntegrationTestChangeSongWeight()
        {
            //Arrange
            int Weight = 9;
            //Act
            songObject.ChangeSongWeight(songObject.ID, Weight);
            //Assert
            var songfromdatabase = songObject.GetSpecificSong(songObject.ID);
            Assert.AreEqual(Weight, songfromdatabase.Weight);
        }
    }
}
