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
        public void RemoveSongFromPlaylist(int playlistID, int songID) 
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM [Playlist_Song] WHERE playlist_ID = @PlaylistID AND song_ID = @SongID";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PlaylistID", playlistID);
                    command.Parameters.AddWithValue("@SongID", songID);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        public void ChangeSongWeight(int songID, int weight) 
        {
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				string query = "UPDATE Song SET weight = weight + @Weight WHERE ID = @SongID";

				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@Weight", weight);
					command.Parameters.AddWithValue("@SongID", songID);

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
                string query = " SELECT * FROM Playlist_Song INNER JOIN Song ON Song.ID = Playlist_Song.song_ID WHERE Playlist_Song.playlist_ID = @playlist_ID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@playlist_ID", playlistID);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(new SongDataModel
                    {
                        ID = (int)reader["ID"],
                        name = reader["Name"].ToString(),
                        weight = (int)reader["Weight"],
                        dateReleased = (DateTime)reader["DateReleased"],
                        artistID = (int)reader["artistID"],
                        albumID = (int)reader["albumID"],
                    });
                }
            }
            return result;
        }
        public List<SongDataModel> GetAllSongs() 
        {
            List<SongDataModel> result = new List<SongDataModel>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = " SELECT * FROM Song";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new SongDataModel
                    {
                        ID = (int)reader["ID"],
                        name = reader["Name"].ToString(),
                        weight = (int)reader["Weight"],
                        dateReleased = (DateTime)reader["DateReleased"],
                        artistID = (int)reader["artistID"],
                        albumID = (int)reader["albumID"],
                    });
                }
            }
            return result;
        }
        public List<SongDataModel> GetSpecificSong(int songID)
        {
            List<SongDataModel> result = new List<SongDataModel>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = " SELECT * FROM Song WHERE ID = @SongID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SongID", songID);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
            }
            return result;
        }

    }
}
