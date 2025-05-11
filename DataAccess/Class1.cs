using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DataAccess
{
	public class DatabaseContext
	{
		private string connectionstringforrealthistimebutactuallynotbecausethisisjustsoidontloseityk = "Server=mssqlstud.fhict.local;Database=dbi562586_i562586;User Id=dbi562586_i562586;Password=Wpb3grVisq;TrustServerCertificate=True;";
		private readonly string connectionString;

		public DatabaseContext(string connectionString)
		{
			this.connectionString = connectionString;
		}
		public SqlConnection GetConnection()
		{
			return new SqlConnection(connectionString);
		}
		public string DatabasePlaylistCreation = @"INSERT INTO Playlist (name, dateAdded, user_ID)";
	}
}
