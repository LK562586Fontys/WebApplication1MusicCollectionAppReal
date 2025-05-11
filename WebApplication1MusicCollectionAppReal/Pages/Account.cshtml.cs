using LogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1MusicCollectionAppReal.Pages
{
    public class Account : PageModel
    {
        public static User CurrentUser { get; set; } = new User { ID = 5, Name = "TestUser" };
		public List<LogicLayer.Playlist> CreatedPlaylists => CurrentUser.userPlaylist;

		public string? Message { get; set; }

        [BindProperty]
        public string NewUsername { get; set; }
        [BindProperty]
        public string NewPassword { get; set; }
        [BindProperty]
        public string NewEmail { get; set; }
        public List<Playlist> Playlists { get; set; }

        public void OnGet()
        {
            LoadUserPlaylists();
        }

        private void LoadUserPlaylists()
        {
            Playlists = CurrentUser.LoadPlaylists();
        }

        public void OnPost()
        {
            CurrentUser.AddPlaylist();
            Message = "Playlist created!";
        }

        public IActionResult OnPostChangeUsername()
        {
                CurrentUser.ChangeUsername(NewUsername);
                return RedirectToPage();
        }

        public IActionResult OnPostChangePassword()
        {
            CurrentUser.ChangePassword(NewPassword);
            return RedirectToPage();
        }

        public IActionResult OnPostChangeEmail()
        {
            CurrentUser.ChangeEmailAddress(NewEmail);
            return RedirectToPage();
        }
    }
}
