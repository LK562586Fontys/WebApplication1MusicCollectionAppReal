using LogicLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace UnitTesting
{
    [TestClass]
    public class SongTests
    {
        private Song songObject;
        [TestInitialize]
        public void Setup()
        {
            songObject = new Song { ID = 5 };
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
