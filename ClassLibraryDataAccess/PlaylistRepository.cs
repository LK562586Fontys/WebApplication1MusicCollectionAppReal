using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DataAccessLayer
{
    public class PlaylistRepository
    {
        private readonly string _connectionString;
        public PlaylistRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void InsertPlaylist(string name, DateTime dateAdded, int creator)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            string insertQuery = @"
			INSERT INTO Playlist (name, dateAdded, Creator)
			VALUES (@name, @date, @creatorId)";

            using SqlCommand insertCmd = new SqlCommand(insertQuery, connection);
            insertCmd.Parameters.AddWithValue("@name", name);
            insertCmd.Parameters.AddWithValue("@date", dateAdded);
            insertCmd.Parameters.AddWithValue("@creatorId", creator);

            insertCmd.ExecuteNonQuery();
        }
        public void UpdatePlaylistName(int playlistId, string newName)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "UPDATE [Playlist] SET name = @name WHERE ID = @ID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", newName);
                    cmd.Parameters.AddWithValue("@ID", playlistId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdatePlaylistPhoto(int playlistId, byte[] newPhoto)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "UPDATE [Playlist] SET picture = @picture WHERE ID = @ID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@picture", SqlDbType.VarBinary).Value = newPhoto;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = playlistId;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<PlaylistDataModel> LoadPlaylists(int userId)
        {
            List<PlaylistDataModel> result = new List<PlaylistDataModel>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Playlist WHERE Creator = @userId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(new PlaylistDataModel
                    {
                        ID = (int)reader["ID"],
                        Name = reader["Name"].ToString(),
                        DateAdded = (DateTime)reader["DateAdded"]
                    });
                }
            }
            return result;
        }
    }
}
