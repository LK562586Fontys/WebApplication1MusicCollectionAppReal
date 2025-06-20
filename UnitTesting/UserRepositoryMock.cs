using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting
{
    public class UserRepositoryMock : IUserRepository
    {
        private List<IUserDataModel> _users = new List<IUserDataModel>
        {
        new UserDTO { ID = 1, Name = "joe" },
        new UserDTO { ID = 2, Name = "louis" }
        };
        public bool CheckEmail(string email)
        {
            throw new NotImplementedException();
        }

        public void DeleteAccount(int userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IUserDataModel> GetAllUsers()
        {
            
            return _users;
        }

        public IUserDataModel? GetSpecificUser(int id)
        {
            if (id == 9)
            {
                return new UserDTO { ID = 9, Name = "TestUser" };
            }
            return null;
        }

        public IEnumerable<IUserDataModel> GetUsersByIds(List<int> userIds)
        {
            throw new NotImplementedException();
        }

        public void UpdateEmail(int userId, string email)
        {
            return;
        }

        public void UpdatePassword(int userId, string newPassword)
        {
            return;
        }

        public void UpdateProfilePhoto(int userId, byte[] picture)
        {
            return;
        }

        public void UpdateUsername(int userId, string newName)
        {
            return;
        }

        public Task<int?> VerifyLoginAndReturnUserId(string email, string password)
        {
            throw new NotImplementedException();
        }

        private class UserDTO : IUserDataModel
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string PasswordHash { get; set; }
            public string EmailAddress { get; set; }
            public byte[] ProfilePhoto { get; set; }
            public DateTime JoinDate { get; set; }

        } 
    }
}
