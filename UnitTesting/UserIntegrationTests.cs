using LogicLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace UnitTesting
{
    [TestClass]
    public class UserIntegrationTests
    {
        private User userObject;
        [TestInitialize]
        public void Setup()
        {
            userObject = new User { ID = 10 };
        }

        [TestMethod]
        public void IntegrationTestChangeUsername()
        {
            //Arrange
            string newname = "MyNewUsername";
            //Act
            userObject.ChangeUsername(newname);
            //Assert
            var userfromdatabase = userObject.GetSpecificUser(userObject.ID);
            Assert.AreEqual(newname, userfromdatabase.Name);
        }

        [TestMethod]
        public void IntegrationTestChangePassword()
        {
            //Arrange
            string newpassword = "MyNewPassword";
            //Act
            userObject.ChangePassword(newpassword);
            //Assert
            var userfromdatabase = userObject.GetSpecificUser(userObject.ID);
            bool isValid = BCrypt.Net.BCrypt.Verify(newpassword, userfromdatabase.PasswordHash);
            Assert.IsTrue(isValid, "The stored bcrypt hash does not match the new password.");
        }
        [TestMethod]
        public void IntegrationTestChangeEmail()
        {
            //Arrange
            string NEWWEMPAIL = "NewEmailADdereres";
            //Act
            userObject.ChangeEmailAddress(NEWWEMPAIL);
            //Assert
            var userfromdatabase = userObject.GetSpecificUser(userObject.ID);
            Assert.AreEqual(NEWWEMPAIL, userObject.EmailAddress);
        }
        [TestMethod]
        public void IntegrationTestChangePFP()
        {
            //Arrange
            byte[] newphoto = { 1, 2, 3, 4 };
            //Act
            userObject.ChangeProfilePhoto(newphoto);
            //Assert
            var userfromdatabase = userObject.GetSpecificUser(userObject.ID);
            CollectionAssert.AreEqual(newphoto, userfromdatabase.ProfilePhoto);
        }
        [TestMethod]
        public void IntegrationTestAddplaylist()
        {
            //Arrange
            DateTime currentdate = DateTime.Now;
            //Act
            userObject.AddPlaylist(currentdate);
            //Assert
            var playlistfromdatabase = userObject.LoadPlaylists();
            Assert.IsNotNull(playlistfromdatabase);
            Assert.AreEqual(1, playlistfromdatabase.Count);
        }
        [TestMethod]
        public void IntegrationTestDeleteAccount()
        {
            //Arrrange
            int skadoosh = userObject.ID;
            //Act
            userObject.DeleteAccount(skadoosh);
            //Assert
            var userfromdatabase = userObject.GetSpecificUser(skadoosh);
            Assert.IsNull(userfromdatabase.Name);
            Assert.IsNull(userfromdatabase.ID);
        }
    }
}
