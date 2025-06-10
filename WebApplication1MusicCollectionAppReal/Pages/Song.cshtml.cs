using Interfaces;
using LogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1MusicCollectionAppReal.Pages
{
    public class Song : PageModel
    {
        private readonly IUserService _userService;
        private readonly IPlaylistService _playlistService;
        private readonly ISongService _songService;
        public static LogicLayer.Song CurrentSong { get; set; }
        public static LogicLayer.Playlist CurrentPlaylist { get; set; }
        private static User CurrentUser { get; set; }
        public Song(IUserService userService, IPlaylistService playlistService, ISongService songService) 
        {
            _userService = userService;
            _playlistService = playlistService;
            _songService = songService;
        }
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
            CurrentUser = (User)_userService.GetUserById((int)userId);
            CurrentSong = (LogicLayer.Song)_songService.GetSongById((int)userId);
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
