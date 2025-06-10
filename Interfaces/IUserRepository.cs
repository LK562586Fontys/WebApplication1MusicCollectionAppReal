namespace Interfaces
{
    public interface IUserRepository
    {
        void UpdateUsername(int userId, string newName);
        void UpdatePassword(int userId, string newPassword);
        void UpdateEmail(int userId, string email);
        void UpdateProfilePhoto(int userId, byte[] picture);
        void DeleteAccount(int userId);
        Task<int?> VerifyLoginAndReturnUserId(string email, string password);
        IUserDTO GetSpecificUser(int userId);
        IEnumerable<IUserDTO> GetUsersByIds(List<int> userIds);
        IEnumerable<IUserDTO> GetAllUsers();
    }
}
