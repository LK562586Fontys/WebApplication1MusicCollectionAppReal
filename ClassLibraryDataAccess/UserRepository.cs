using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;


namespace DataAccessLayer
{
    public class UserRepository
    {
        public readonly string _connectionString;
        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
		//public UserRepository(IConfiguration configuration)
		//{
		//	_connectionString = configuration.GetConnectionString("DefaultConnection");
		//}
		public void UpdateUsername(int userId, string newUsername)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE [User] SET userName = @userName WHERE ID = @ID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userName", newUsername);
                    command.Parameters.AddWithValue("@ID", userId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        public void UpdatePassword(int userId, string newPassword)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE [User] SET passwordHash = @passwordHash WHERE ID = @ID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
					string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

					command.Parameters.AddWithValue("@passwordHash", hashedPassword);
                    command.Parameters.AddWithValue("@ID", userId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        public void UpdateEmail(int userId, string newEmail)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE [User] SET emailAddress = @emailAddress WHERE ID = @ID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@emailAddress", newEmail);
                    command.Parameters.AddWithValue("@ID", userId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        public void UpdateProfilePhoto(int userId, byte[] newPhoto)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE [User] SET profilePicture = @picture WHERE ID = @ID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@picture", SqlDbType.VarBinary).Value = newPhoto;
                    command.Parameters.Add("@ID", SqlDbType.Int).Value = userId;

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        public void DeleteAccount(int userID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE [Playlist] SET creator = NULL WHERE creator = @ID;
DELETE FROM [User] WHERE ID = @ID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", userID);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        public List<UserDataModel> GetSpecificUser(int userID)
        {
            List<UserDataModel> result = new List<UserDataModel>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = " SELECT * FROM [User] WHERE ID = @UserID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userID);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    var user = new UserDataModel
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        userName = reader["Username"].ToString(),
                        email = reader["emailAddress"].ToString(),
                        joinDate = (DateTime)reader["joinDate"],
                        password = reader["passwordHash"].ToString(),
                        picture = reader["profilePicture"] != DBNull.Value ? (byte[])reader["profilePicture"] : null
                    };

                    result.Add(user);
                }
            }
            return result;
        }
        public List<UserDataModel> GetUsersByIds(List<int> userIds)
        {
            var users = new List<UserDataModel>();

            if (userIds == null || userIds.Count == 0)
                return users;

            string ids = string.Join(",", userIds);

            string query = $"SELECT * FROM [User] WHERE ID IN ({ids})";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new UserDataModel
                        {
                            ID = (int)reader["ID"],
                            userName = reader["userName"].ToString()
                        });
                    }
                }
            }

            return users;
        }
        public List<UserDataModel> GetAllUsers()
        {
            List<UserDataModel> result = new List<UserDataModel>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = " SELECT * FROM [User]";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new UserDataModel
                    {
                        ID = (int)reader["ID"],
                        userName = reader["userName"].ToString(),
                        password = reader["passwordHash"].ToString(),
                        email = reader["emailAddress"].ToString(),
                        joinDate = (DateTime)reader["joinDate"],
                    });
                }
            }
            return result;
        }
		public async Task<int?> VerifyLoginAndReturnUserId(string emailAddress, string inputPassword)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				var cmd = new SqlCommand("SELECT ID, passwordHash FROM [user] WHERE emailAddress = @Email", connection);
				cmd.Parameters.AddWithValue("@Email", emailAddress);

				await connection.OpenAsync();
				using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
				{
					if (await reader.ReadAsync())
					{
						var storedHash = reader["passwordHash"]?.ToString();

						// Check if the hash is null or invalid before verifying
						if (!string.IsNullOrEmpty(storedHash) && BCrypt.Net.BCrypt.Verify(inputPassword, storedHash))
						{
							return Convert.ToInt32(reader["ID"]);
						}
					}
				}
			}
			return null;
		}
	}
}
