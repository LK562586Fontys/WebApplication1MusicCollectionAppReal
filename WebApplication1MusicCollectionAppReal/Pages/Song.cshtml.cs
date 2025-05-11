using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1MusicCollectionAppReal.Pages
{
    public class Song : PageModel
    {
        public static LogicLayer.Song CurrentSong { get; set; } = new LogicLayer.Song { ID = 5 };
        public static LogicLayer.Playlist CurrentPlaylist { get; set; } = new LogicLayer.Playlist { ID = 6 };
        public void OnGet()
        {
        }

        public void OnPost() 
        {
            CurrentPlaylist.AddSong(CurrentSong.ID);
        }

    }
}
