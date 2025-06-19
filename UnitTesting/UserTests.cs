using Interfaces;
using LogicLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace UnitTesting
{
    [TestClass]
    public class UserTests
    {
        private UserFactory userService;
        private User? userObject;
        [TestInitialize]
        public void Setup()
        {
			var songRepoMock = new SongRepositoryMock();
			var userRepoMock = new UserRepositoryMock();
			var playlistRepoMock = new PlaylistRepositoryMock();
			userService = new UserFactory(userRepoMock, playlistRepoMock, songRepoMock);

            userObject = userService.GetUserById(9) as User;
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
            string newpassword = "MyNewPassword";
            //Act
            userObject.ChangePassword(newpassword);
            //Assert
            Assert.AreEqual(newpassword, userObject.PasswordHash);
        }
        [TestMethod]
        public void TestChangeEmail() 
        {
            //Arrange
            string newemail = "NewEmail@testing.com";
            //Act
            userObject.ChangeEmailAddress(newemail);
            //Assert
            Assert.AreEqual(newemail, userObject.EmailAddress);
        }
        [TestMethod]
        public void TestChangeProfilePicture() 
        {
            //Arrange
            byte[] newphoto = { 1, 2, 3, 4 };
            //Act
            userObject.ChangeProfilePhoto(newphoto);
            //Assert
            Assert.AreEqual(newphoto, userObject.ProfilePhoto);
        }
        [TestMethod]
        public void TestAddplaylist() 
        {
            //Arrange
            DateTime currentdate = DateTime.Now;
            var fakePlaylistRepo = new PlaylistRepositoryMock();
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
        [TestMethod]
        public void ChangeUsernameExceptionHandling()
        {
            // Arrange
            string newname = "A";

            // Act & Assert
            var ex = Assert.ThrowsException<ArgumentException>(() => userObject.ChangeUsername(newname));
            Assert.AreEqual("The username must be between 2 and 50 characters long.", ex.Message);
        }
        [TestMethod]
        public void ChangePasswordExceptionHandling()
        {
            // Arrange
            string newpassword = "B";

            // Act & Assert
            var ex = Assert.ThrowsException<ArgumentException>(() => userObject.ChangePassword(newpassword));
            Assert.AreEqual("Your password must be above 7 characters", ex.Message);
        }
        [TestMethod]
        public void ChangeEmailExceptionHandlingPart1() 
        {
            // Arrange
            string newemail = "j@j.j";

            // Act & Assert
            var ex = Assert.ThrowsException<ArgumentException>(() => userObject.ChangeEmailAddress(newemail));
            Assert.AreEqual("Your E-mail must be above 7 characters", ex.Message);
        }

        [TestMethod]
        public void ChangeEmailExceptionHandlingPart2()
        {
            // Arrange
            string newemail = "ThisIsTotallyTheEmailThatIAmGoingToUse";

            // Act & Assert
            var ex = Assert.ThrowsException<ArgumentException>(() => userObject.ChangeEmailAddress(newemail));
            Assert.AreEqual("Your E-mail must include '@' and '.'", ex.Message);
        }
    }
}