using LogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1MusicCollectionAppReal.Pages.ViewModels;

namespace WebApplication1MusicCollectionAppReal.Pages
{
    public class Account : PageModel
    {
        public AccountViewModel ViewModel { get; set; }
        private static User CurrentUser { get; set; }
		public List<LogicLayer.Playlist> CreatedPlaylists => CurrentUser.userPlaylist;
        public IFormFile NewPhoto { get; set; }

        [BindProperty]
        public string NewUsername { get; set; }
        [BindProperty]
        public string NewPassword { get; set; }
        [BindProperty]
        public string NewEmail { get; set; }
        public List<Playlist> Playlists { get; set; }

        public void OnGet(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                Response.Redirect("/Login");
                return;
            }
            ViewModel = ViewModel ?? new AccountViewModel();
            if (id != 0)
            {
                CurrentUser = new User { ID = id };
            }
            else {
                CurrentUser = new User { ID = userId.Value };
            }
            CurrentUser.GetSpecificUser(CurrentUser.ID);
            LoadUserPlaylists(); // this can still use CurrentUser internally

            ViewModel.Name = CurrentUser.Name;
            ViewModel.EmailAddress = CurrentUser.EmailAddress;
            ViewModel.joinDate = CurrentUser.joinDate;
            ViewModel.Playlists = CurrentUser.userPlaylist;

            if (CurrentUser.ProfilePhoto != null)
            {
                ViewModel.ProfileBase64Photo = Convert.ToBase64String(CurrentUser.ProfilePhoto);
            }

            foreach (var playlist in ViewModel.Playlists)
            {
                if (playlist.Photo != null)
                {
                    playlist.Base64Photo = Convert.ToBase64String(playlist.Photo);
                }
            }
        }

        private void LoadUserPlaylists()
        {
            var cookie = Request.Cookies["PlaylistOrder"];
            Playlists = CurrentUser.LoadPlaylists(cookie);
            
        }

        public IActionResult OnPostAddPlaylist()
        {
            DateTime CurrentDate = DateTime.Now;
            CurrentUser.AddPlaylist(CurrentDate);
            return RedirectToPage();
        }

        public IActionResult OnPostChangeUsername()
        {
            if (NewUsername.Length > 3 && NewUsername.Length < 50)
            {
                CurrentUser.ChangeUsername(NewUsername);
            }
            return RedirectToPage();
        }

        public IActionResult OnPostChangePassword()
        {
            if (NewPassword.Length > 7) 
            { 
                CurrentUser.ChangePassword(NewPassword);
            }
            return RedirectToPage();
        }

        public IActionResult OnPostChangeEmail()
        {
            if (NewEmail.Length > 10 && NewEmail.Contains("@") && NewEmail.Contains(".")) 
            {
            CurrentUser.ChangeEmailAddress(NewEmail);
            }
            return RedirectToPage();
        }

        public IActionResult OnPostSignOut()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Login"); 
        }

        public IActionResult OnPostDeleteAccount()
        {
            CurrentUser.DeleteAccount(CurrentUser.ID);
            HttpContext.Session.Clear();
            return RedirectToPage("/Login");
        }
        public async Task<IActionResult> OnPostChangeProfilePhoto()
        {
            using var memoryStream = new MemoryStream();
            await NewPhoto.CopyToAsync(memoryStream);
            byte[] imageBytes = memoryStream.ToArray();
            CurrentUser.ChangeProfilePhoto(imageBytes);
            return RedirectToPage();
        }
        public IActionResult OnPostReorder(string move, List<int> playlistOrder)
        {
            if (playlistOrder == null || playlistOrder.Count == 0 || string.IsNullOrEmpty(move))
                return RedirectToPage();

            var parts = move.Split('_');
            if (parts.Length != 2) return RedirectToPage();

            string direction = parts[0];
            if (!int.TryParse(parts[1], out int index)) return RedirectToPage();

            if (direction == "up" && index > 0)
            {
                (playlistOrder[index], playlistOrder[index - 1]) = (playlistOrder[index - 1], playlistOrder[index]);
            }
            else if (direction == "down" && index < playlistOrder.Count - 1)
            {
                (playlistOrder[index], playlistOrder[index + 1]) = (playlistOrder[index + 1], playlistOrder[index]);
            }

            Response.Cookies.Append("PlaylistOrder", string.Join(",", playlistOrder), new CookieOptions
            {
                Expires = DateTime.Now.AddDays(2)
            });

            return RedirectToPage();
        }
    }
}
