using LogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1MusicCollectionAppReal.Pages
{
    public class Song : PageModel
    {
        public static LogicLayer.Song CurrentSong { get; set; } = new LogicLayer.Song { ID = 6 };
        public static LogicLayer.Playlist CurrentPlaylist { get; set; } = new LogicLayer.Playlist { ID = 6 };
        public void OnGet()
        {
            CurrentSong.GetSpecificSong(CurrentSong.ID);
        }

        public IActionResult OnPostAddSongToPlaylist() 
        {
            CurrentPlaylist.AddSong(CurrentSong.ID);
            return RedirectToPage();
        }
		public IActionResult OnPostUpdateWeight(int Weight)
		{
			CurrentSong.ChangeSongWeight(CurrentSong.ID, Weight);
			return RedirectToPage();
		}
        public IActionResult OnPostRemoveSongFromPlaylist()
        {
            CurrentPlaylist.RemoveSong(CurrentSong.ID);
            return RedirectToPage();
        }
    }
}
