
using LogicLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace UnitTesting
{
    [TestClass]
    public class UserTests
    {
        private User userObject;
        [TestInitialize]
        public void Setup()
        {
            userObject = new User { ID = 3};
        }

        [TestMethod]
        public void TestChangeUsername()
        {
            //Arrange
            string newname = "MyNewUsername";
            //Act
            userObject.ChangeUsername(newname);
            //Assert
            Assert.AreEqual(newname, userObject.Name);
        }

        [TestMethod]
        public void TestChangePassword()
        {
            //Arrange
            string newpassword = "MyNewUsername";
            //Act
            userObject.ChangePassword(newpassword);
            //Assert
            Assert.AreEqual(newpassword, userObject.PasswordHash);
        }
        [TestMethod]
        public void TestChangeEmail() 
        {
            //Arrange
            string NEWWEMPAIL = "NewEmailADdereres";
            //Act
            userObject.ChangeEmailAddress(NEWWEMPAIL);
            //Assert
            Assert.AreEqual(NEWWEMPAIL, userObject.EmailAddress);
        }
        [TestMethod]
        public void TestChangePFP() 
        {
            //Arrange
            byte[] newphoto = { 1, 2, 3, 4 };
            //Act
            userObject.ChangeProfilePhoto(newphoto);
            //Assert
            Assert.AreEqual(newphoto, userObject.ProfilePhoto);
        }
        [TestMethod]
        public void TestAddplaylsit() 
        {
            //Arrange
            DateTime currentdate = DateTime.Now;
            //Act
            userObject.AddPlaylist(currentdate);
            //Assert
            Assert.AreEqual(1, userObject.userPlaylist.Count);
            Assert.AreEqual(currentdate, userObject.userPlaylist[0].DateAdded);
        }
        [TestMethod]
        public void TestDeleteAccount()
        {
            //Arrrange
            int skadoosh = userObject.ID;
            //Act
            userObject.DeleteAccount(skadoosh);
            //Assert
            Assert.IsNull(userObject.Name);
            Assert.IsNull(userObject.EmailAddress);
        }
    }
}