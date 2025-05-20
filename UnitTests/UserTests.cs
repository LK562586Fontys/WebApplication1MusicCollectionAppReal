using LogicLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace UnitTests
{
    [TestClass]
    public class UserTests
    {
        private User userObject;
        [TestInitialize]
        public void Setup() 
        {
            userObject = new User();
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
    }
}
