using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public void UpdateUsername(int userId, string newUsername)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "UPDATE [User] SET userName = @userName WHERE ID = @ID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userName", newUsername);
                    cmd.Parameters.AddWithValue("@ID", userId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdatePassword(int userId, string newPassword)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "UPDATE [User] SET passwordHash = @passwordHash WHERE ID = @ID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@passwordHash", newPassword);
                    cmd.Parameters.AddWithValue("@ID", userId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdateEmail(int userId, string newEmail)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "UPDATE [User] SET emailAddress = @emailAddress WHERE ID = @ID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@emailAddress", newEmail);
                    cmd.Parameters.AddWithValue("@ID", userId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
