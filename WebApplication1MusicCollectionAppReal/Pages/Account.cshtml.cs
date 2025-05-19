using LogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1MusicCollectionAppReal.Pages
{
    public class Account : PageModel
    {
        public static User CurrentUser { get; set; } = new User { ID = 11, Name = "TestUser" };
		public List<LogicLayer.Playlist> CreatedPlaylists => CurrentUser.userPlaylist;
        public IFormFile NewPhoto { get; set; }
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

        public IActionResult OnPostAddPlaylist()
        {
            DateTime CurrentDate = DateTime.Now;
            CurrentUser.AddPlaylist(CurrentDate);
            Message = "Playlist created!";
            return RedirectToPage();
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

        public IActionResult OnPostDeleteAccount()
        {
            CurrentUser.DeleteAccount(CurrentUser.ID);
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostChangeProfilePhoto()
        {
            using var memoryStream = new MemoryStream();
            await NewPhoto.CopyToAsync(memoryStream);
            byte[] imageBytes = memoryStream.ToArray();
            CurrentUser.ChangeProfilePhoto(imageBytes);
            return RedirectToPage();
        }
    }
}
