using Interfaces;
using LogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1MusicCollectionAppReal.Pages.ViewModels;

namespace WebApplication1MusicCollectionAppReal.Pages
{
    public class Account : PageModel
    {
        private readonly ISongRepository _songRepository;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IUserService _userService;
        public AccountViewModel ViewModel { get; set; }
        public string ErrorMessage { get; set; }
        private static User CurrentUser { get; set; }
        public IFormFile NewPhoto { get; set; }

        [BindProperty]
        public string NewUsername { get; set; }
        [BindProperty]
        public string NewPassword { get; set; }
        [BindProperty]
        public string NewEmail { get; set; }
        public List<Playlist> Playlists { get; set; }

        public Account(IUserService userService, IPlaylistRepository playlistRepository, ISongRepository songRepository) 
        {
            _userService = userService;
            _playlistRepository = playlistRepository;
            _songRepository = songRepository;
        }
        public IActionResult OnGet(int? id)
        {
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                return RedirectToPage("/Login");
            }
            
            if (id != null)
            {
                CurrentUser = (User)_userService.GetUserById((int)id);
            }
            else {
                CurrentUser = (User)_userService.GetUserById((int)userId);
            }
            LoadUserPlaylists();
            ViewModel = new AccountViewModel
            {
                Name = CurrentUser.Name,
                EmailAddress = CurrentUser.EmailAddress,
                joinDate = CurrentUser.joinDate,
                Playlists = CurrentUser.userPlaylist,
                ProfilePhoto = CurrentUser.ProfilePhoto,
            };
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

            return Page();
        }

        private void LoadUserPlaylists()
        {
            var cookie = Request.Cookies["PlaylistOrder"];
            Playlists = CurrentUser.LoadPlaylists(_playlistRepository, _songRepository, cookie);
            
        }

        public IActionResult OnPostAddPlaylist()
        {
            DateTime CurrentDate = DateTime.Now;
            CurrentUser.AddPlaylist(CurrentDate, _playlistRepository);
            return RedirectToPage();
        }

        public IActionResult OnPostChangeUsername()
        {
            //CurrentUser.ChangeUsername(NewUsername);
            //return RedirectToPage();
            try
            {
                // Try to change the username
                CurrentUser.ChangeUsername(NewUsername);

                // If successful, redirect to the same page or another page
                return RedirectToPage();
            }
            catch (ArgumentException ex)
            {
                // If the username is invalid, handle the exception
                ErrorMessage = ex.Message;  // Pass the error message to the view

                // Stay on the same page and show the error message
                return Page();
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                ErrorMessage = "An unexpected error occurred. Please try again later.";
                // Optionally log the exception for debugging
                return Page();
            }
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
