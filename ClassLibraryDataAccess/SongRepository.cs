using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class SongRepository
    {
        private readonly string _connectionString;

        public SongRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddSongToPlaylist(int playlistId, int songId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Playlist_Song (playlist_ID, song_ID) VALUES (@PlaylistID, @SongID)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PlaylistID", playlistId);
                    command.Parameters.AddWithValue("@SongID", songId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        public List<SongDataModel> GetSongList(int playlistID) 
        {
            List<SongDataModel> result = new List<SongDataModel>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM [Playlist_Song] INNER JOIN Playlist_Song ON Playlist.ID =      WHERE playlist_ID = @playlist_ID";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@playlist_ID", playlistID);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(new SongDataModel
                    {
                        ID = (int)reader["ID"],
                        name = reader["Name"].ToString(),
                        weight = (int)reader["Weight"],
                        dateReleased = (DateTime)reader["DateReleased"]
                    });
                }
            }
            return result;
        }
    }
}
