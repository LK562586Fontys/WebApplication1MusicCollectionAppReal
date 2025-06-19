namespace Interfaces
{
    public interface IUserRepository
    {
        void UpdateUsername(int userId, string newName);
        void UpdatePassword(int userId, string newPassword);
        void UpdateEmail(int userId, string email);
        void UpdateProfilePhoto(int userId, byte[] picture);
        void DeleteAccount(int userId);
        bool CheckEmail(string email);
        Task<int?> VerifyLoginAndReturnUserId(string email, string password);
        IUserDataModel GetSpecificUser(int userId);
        IEnumerable<IUserDataModel> GetUsersByIds(List<int> userIds);
        IEnumerable<IUserDataModel> GetAllUsers();
    }
}
