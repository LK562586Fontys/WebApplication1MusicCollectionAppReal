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
        public void DeleteAccount(int userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IUserDTO> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public IUserDTO? GetSpecificUser(int id)
        {
            if (id == 9)
            {
                return new UserDTO { ID = 9, Name = "TestUser" };
            }
            return null;
        }

        public IEnumerable<IUserDTO> GetUsersByIds(List<int> userIds)
        {
            throw new NotImplementedException();
        }

        public void UpdateEmail(int userId, string email)
        {
            throw new NotImplementedException();
        }

        public void UpdatePassword(int userId, string newPassword)
        {
            throw new NotImplementedException();
        }

        public void UpdateProfilePhoto(int userId, byte[] picture)
        {
            throw new NotImplementedException();
        }

        public void UpdateUsername(int userId, string newName)
        {
            throw new NotImplementedException();
        }

        public Task<int?> VerifyLoginAndReturnUserId(string email, string password)
        {
            throw new NotImplementedException();
        }

        // Simple DTO class for mocking purposes
        private class UserDTO : IUserDTO
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string PasswordHash { get; set; }
            public string EmailAddress { get; set; }
            public byte[] ProfilePhoto { get; set; }
            public DateTime joinDate { get; set; }

        } 
    }
}
