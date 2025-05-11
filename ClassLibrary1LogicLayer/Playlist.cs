using DataAccessLayer;

namespace LogicLayer
{
    public class Playlist
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime DateAdded { get; set; }
        public byte[] Photo { get; set; }
        public List<Song> PlaylistSongs { get; set; }
        public User Creator { get; set; }
        private SongRepository rep = new SongRepository("Server=mssqlstud.fhict.local;Database=dbi562586_i562586;User Id=dbi562586_i562586;Password=Wpb3grVisq;TrustServerCertificate=True;");
        PlaylistRepository db = new PlaylistRepository("Server=mssqlstud.fhict.local;Database=dbi562586_i562586;User Id=dbi562586_i562586;Password=Wpb3grVisq;TrustServerCertificate=True;");
        public void ChangePlaylistPicture(byte[] newPhoto)
        {
            Photo = newPhoto; // Update the property if needed
            db.UpdatePlaylistPhoto(ID, newPhoto);
        }

        public void ChangePlaylistName(string newName)
        {
            Name = newName;
            db.UpdatePlaylistName(ID, newName);
        }

        public void AddSong(int songid) //Must
        {
            
            rep.AddSongToPlaylist(ID, songid);
        }

        private void RemoveSong()
        {
            //Add a song to this playlist
        }

        private void SortSong()
        {
            //Sort songs in this playlist
        }

        private void ShuffleSong()
        {
            //Shuffle songs in this playlist
        }
        public List<Song> LoadSongs() 
        {
            var songdata = rep.GetSongList(ID);
            List<Song> songs = new List<Song>();
            foreach (var item in songdata)
            {
                songs.Add(new Song
                {
                    ID = item.ID,
                    Name = item.name,
                    Weight = item.weight,
                    DateReleased = item.dateReleased,
                });
            }
            this.PlaylistSongs = songs;
            return songs;

        }
    }
}
