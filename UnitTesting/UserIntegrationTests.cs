using LogicLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccessLayer;
using Interfaces;
namespace UnitTesting
{
    [TestClass]
    public class UserIntegrationTests
    {
        private User userObject;
        private UserFactory _userService;
        private UserRepository _userRepository;
        private PlaylistRepository _playlistRepository;
        private SongRepository _songRepository;
        [TestInitialize]
        public void Setup()
        {
            _userRepository = new UserRepository("Server=mssqlstud.fhict.local;Database=dbi562586_i562586;User Id=dbi562586_i562586;Password=Wpb3grVisq;TrustServerCertificate=True;"); // Use the actual implementation for integration tests

            _playlistRepository = new PlaylistRepository("Server=mssqlstud.fhict.local;Database=dbi562586_i562586;User Id=dbi562586_i562586;Password=Wpb3grVisq;TrustServerCertificate=True;");
            _songRepository = new SongRepository("Server=mssqlstud.fhict.local;Database=dbi562586_i562586;User Id=dbi562586_i562586;Password=Wpb3grVisq;TrustServerCertificate=True;");
			_userService = new UserFactory(_userRepository, _playlistRepository, _songRepository);
			userObject = _userService.GetUserById(8)!;
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
            Assert.IsTrue(isValid, "The stored bcrypt hash does not match the new PasswordHash.");
        }
        [TestMethod]
        public void IntegrationTestChangeEmail()
        {
            //Arrange
            string newemail = "NewEmail@testing.com";
            //Act
            userObject.ChangeEmailAddress(newemail);
            //Assert
            var userfromdatabase = userObject.GetSpecificUser(userObject.ID);
            Assert.AreEqual(newemail, userObject.EmailAddress);
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
            int userid = userObject.ID;
            //Act
            userObject.DeleteAccount(userid);
            //Assert
            var userfromdatabase = userObject.GetSpecificUser(userid);
            Assert.IsNull(userfromdatabase);

        }
        [TestMethod]
        public void ChangeEmailExceptionHandling() 
        {
            //Arrange
            string alreadytakenemail = "user7@example.com";

            //Act & Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(() => userObject.ChangeEmailAddress(alreadytakenemail));
            Assert.AreEqual("The E-mail provided is already in use", ex.Message);
        }
    }
}
