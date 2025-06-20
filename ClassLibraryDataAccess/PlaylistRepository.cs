using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DataAccessLayer
{
    public class PlaylistRepository : Interfaces.IPlaylistRepository
    {
        private readonly string _connectionString;
        public PlaylistRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public PlaylistRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public void InsertPlaylist(string name, DateTime dateAdded, int creator)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string insertQuery = @"
			    INSERT INTO Playlist (name, dateAdded, Creator)
			    VALUES (@name, @date, @creatorId)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@date", dateAdded);
                    command.Parameters.AddWithValue("@creatorId", creator);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        public void UpdatePlaylistName(int playlistId, string newName)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE [Playlist] SET name = @name WHERE ID = @ID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", newName);
                    command.Parameters.AddWithValue("@ID", playlistId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
		public void DeletePlaylist(int playlistID)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				string query = @"DELETE FROM [Playlist_Song] WHERE playlist_ID = @ID;
                DELETE FROM [Playlist] WHERE ID = @ID";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@ID", playlistID);

					connection.Open();
					command.ExecuteNonQuery();
				}
			}
		}
		public void UpdatePlaylistPhoto(int playlistId, byte[] newPhoto)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE [Playlist] SET picture = @picture WHERE ID = @ID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@picture", SqlDbType.VarBinary).Value = newPhoto;
                    command.Parameters.Add("@ID", SqlDbType.Int).Value = playlistId;

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        public void SortSongs(int playlistID, string field, string value) 
        {
            using (SqlConnection connection = new SqlConnection(_connectionString)) 
            {
                string query = $@"
                SELECT Song.ID, Song.songTitle, Song.songDuration, Song.songGenre, Song.AlbumID, Song.ArtistID, Album.playlistName AS AlbumName, Artist.Name AS ArtistName FROM Playlist_Song
                INNER JOIN Song ON Song.ID = Playlist_Song.song_ID
                INNER JOIN Playlist AS Album ON Song.AlbumID = Album.ID
                INNER JOIN [User] AS Artist ON Song.ArtistID = Artist.ID
                WHERE Playlist_Song.playlist_ID = @playlist_ID
                ORDER BY {field} {value}";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("playlist_ID", playlistID);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                }
            }
        }
        public IEnumerable<IPlaylistDataModel> LoadPlaylists(int userId)
        {
            List<PlaylistDataModel> result = new();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Playlist WHERE Creator = @userId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@userId", userId);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(new PlaylistDataModel
                    {
                        ID = (int)reader["ID"],
                        Name = reader["name"].ToString(),
                        DateAdded = (DateTime)reader["dateAdded"],
                        Photo = reader["picture"] != DBNull.Value ? (byte[])reader["picture"] : null
                    });
                }
            }
            return result;
        }

        public IPlaylistDataModel GetPlaylistById(int id)
		{
			PlaylistDataModel playlist = null;

			string query = "SELECT * FROM [Playlist] WHERE ID = @id";
			using (SqlConnection connection = new SqlConnection(_connectionString))
			using (SqlCommand command = new SqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@id", id);
				connection.Open();

				using (SqlDataReader reader = command.ExecuteReader())
				{
					if (reader.Read())
					{
						playlist = new PlaylistDataModel
						{
							ID = (int)reader["ID"],
							Name = reader["Name"].ToString(),
                            DateAdded = (DateTime)reader["dateAdded"],
                            Photo = reader["picture"] != DBNull.Value ? (byte[])reader["picture"] : null,
							Creator = new UserDataModel { ID = (int)reader["creator"] }
						};
					}
				}
			}

			return playlist;
		}

		public IEnumerable<IPlaylistDataModel> GetPlaylistsByIds(List<int> playlistIds)
        {
            var playlists = new List<PlaylistDataModel>();

            if (playlistIds == null || playlistIds.Count == 0)
                return playlists;

            string ids = string.Join(",", playlistIds);

            string query = $"SELECT * FROM [Playlist] WHERE ID IN ({ids})";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        playlists.Add(new PlaylistDataModel
                        {
                            ID = (int)reader["ID"],
                            Name = reader["Name"].ToString()
                        });
                    }
                }
            }

            return playlists;
        }
        public IEnumerable<IPlaylistDataModel> GetAllPlaylists()
        {
            List<PlaylistDataModel> result = new List<PlaylistDataModel>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = " SELECT * FROM Playlist";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new PlaylistDataModel
                    {
                        ID = (int)reader["ID"],
                        Name = reader["name"].ToString(),
                        Creator = reader["creator"] != DBNull.Value ? new UserDataModel { ID = Convert.ToInt32(reader["creator"]) } : null,
                        DateAdded = (DateTime)reader["dateAdded"],
                        Photo = reader["picture"] != DBNull.Value ? (byte[])reader["picture"] : null,
                    });
                }
            }
            return result;
        }
    }
}
