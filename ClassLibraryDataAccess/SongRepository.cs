using Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class SongRepository : Interfaces.ISongRepository
    {
        private readonly string _connectionString;

        public SongRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SongRepository(IConfiguration configuration) 
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
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

        public bool SongPlaylistCheck(int playlistID, int songID) 
        {
            using (var connection = new SqlConnection(_connectionString)) 
            {
                string query = "SELECT COUNT(*) FROM [Playlist_Song] WHERE playlist_ID = @PlaylistID AND song_ID = @SongID";
                using SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@PlaylistID", playlistID);
                cmd.Parameters.AddWithValue("@SongID", songID);

                connection.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
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
        public IEnumerable<ISongDTO> GetSongList(int playlistID) 
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
                        Name = reader["Name"].ToString(),
                        Weight = (int)reader["Weight"],
                        DateReleased = (DateTime)reader["DateReleased"],
                        Artist = new UserDataModel { ID = (int)reader["artistID"] },
                        Album = new PlaylistDataModel { ID = (int)reader["albumID"] },
                    });
                }
            }
            return result;
        }
        public IEnumerable<ISongDTO> SearchSongs(string searchTerm) 
        {
            List<SongDataModel> result = new List<SongDataModel>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Song WHERE name LIKE @Search";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Search", "%" + searchTerm + "%");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new SongDataModel
                    {
                        ID = (int)reader["ID"],
                        Name = reader["Name"].ToString(),
                        Weight = (int)reader["Weight"],
                        DateReleased = (DateTime)reader["DateReleased"],
                        Artist = new UserDataModel { ID = (int)reader["artistID"] },
                        Album = new PlaylistDataModel { ID = (int)reader["albumID"] },
                    });
                }
            }
            return result;
        }
        public ISongDTO GetSpecificSong(int songID)
        {
            SongDataModel song = null;

            string query = "SELECT * FROM Song WHERE ID = @SongID";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@SongID", songID);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read()) // If there is at least one row
                    {
                        song = new SongDataModel
                        {
                            ID = (int)reader["ID"],
                            Name = reader["name"].ToString(),
                            Weight = reader["weight"] != DBNull.Value ? (int)reader["weight"] : 0,
                            DateReleased = reader["dateReleased"] != DBNull.Value ? (DateTime)reader["dateReleased"] : DateTime.MinValue,
                            Artist = new UserDataModel { ID = (int)reader["artistID"] },
                            Album = new PlaylistDataModel { ID = (int)reader["albumID"] },
                        };
                    }
                }
            }

            return song;
        }

    }
}
