namespace WebApplication1MusicCollectionAppReal
{
    public class Playlist
    {
        private int ID { get; set; }
        private string name { get; set; }
        private DateTime dateAdded { get; set; }
        private string photo { get; set; }
        private User userID { get; set; }
        private List<Song> PlaylistSongs { get; set; }
        private User User_ID { get; set; }
        private void ChangePlaylistPicture(string newPhoto)
        {
            //change the picture on the playlist
            newPhoto = "IMG_9237_edit_final2.jpg";
            if (newPhoto.Contains(".jpg") || newPhoto.Contains(".jpeg") || newPhoto.Contains(".png"))
            { photo = newPhoto; }
            else Console.WriteLine("wrong thing file");
        }
        private void ChangePlaylistName(string newName)
        {
            //change the name on the playlist
            //newName = textboxbutonlineinputfillthingy.text
            if (newName.Length !> 3 || newName.Length !< 50) { newName = newName; }
            else return;
        }
        private void AddSong() 
        {
            //Add a song to this playlist
            Song newSong = new Song();
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
    }
}
