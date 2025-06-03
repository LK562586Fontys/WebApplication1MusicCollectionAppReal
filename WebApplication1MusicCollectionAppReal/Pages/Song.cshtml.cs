using LogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1MusicCollectionAppReal.Pages
{
    public class Song : PageModel
    {
        public static LogicLayer.Song CurrentSong { get; set; } = new LogicLayer.Song { ID = 6 };
        public static LogicLayer.Playlist CurrentPlaylist { get; set; } = new LogicLayer.Playlist { ID = 6 };
        public User CurrentUser { get; set; }
        public void OnGet(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                // Not logged in — redirect to login
                Response.Redirect("/Login");
                return;
            }

            // Set the current user
            CurrentUser = new User { ID = userId.Value };
            CurrentSong = new LogicLayer.Song { ID = id };
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
