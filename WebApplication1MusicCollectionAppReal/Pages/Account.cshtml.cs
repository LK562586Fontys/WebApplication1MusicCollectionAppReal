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
        public string? NewUsername { get; set; }
        [BindProperty]
        public string? NewPassword { get; set; }
        [BindProperty]
        public string? NewEmail { get; set; }
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
            if (CurrentUser == null) 
            {
                return RedirectToPage("/Error", new { message = "User not found" });
            }
            LoadUserPlaylists();
            return Page();
        }

        private void LoadUserPlaylists()
        {
            var cookie = Request.Cookies["PlaylistOrder"];
            Playlists = CurrentUser.LoadPlaylists(cookie);

            ViewModel = new AccountViewModel
            {
                ID = CurrentUser.ID,
                Name = CurrentUser.Name,
                EmailAddress = CurrentUser.EmailAddress,
                joinDate = CurrentUser.JoinDate,
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

        }

        public IActionResult OnPostAddPlaylist()
        {
            DateTime CurrentDate = DateTime.Now;
            try
            {
                CurrentUser.AddPlaylist(CurrentDate);
                LoadUserPlaylists();
                return Page();
            }
            catch (Exception ex) 
            {
                ErrorMessage = "An unexpected error has occurred. Please try again later";
                LoadUserPlaylists();
                return Page();
            }
        }

        public IActionResult OnPostChangeUsername()
        {
            try
            {
                CurrentUser.ChangeUsername(NewUsername);

                LoadUserPlaylists();
                return Page();
            }
            catch (ArgumentException ex)
            {
                ErrorMessage = ex.Message;

                LoadUserPlaylists();
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = "An unexpected error occurred. Please try again later.";

                LoadUserPlaylists();
                return Page();
            }
        }

        public IActionResult OnPostChangePassword()
        {
            try
            {
                CurrentUser.ChangePassword(NewPassword);
                LoadUserPlaylists();
                return Page();
            }
            catch (ArgumentException ex)
            {
                ErrorMessage = ex.Message;
                LoadUserPlaylists();
                return Page();
            }
            catch (Exception ex) 
            {
                ErrorMessage = "An unexpected error occurred. Please try again later";
                LoadUserPlaylists();
                return Page();
            }
        }

        public IActionResult OnPostChangeEmail()
        {
            try
            {
                CurrentUser.ChangeEmailAddress(NewEmail);
                LoadUserPlaylists();
                return Page();
            }
            catch (ArgumentException ex) 
            {
                ErrorMessage = ex.Message;
                LoadUserPlaylists();
                return Page();
            }
            catch (Exception ex) 
            {
                ErrorMessage = "An unexpected error occurred. Please try again later";
                LoadUserPlaylists();
                return Page();
            }
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
            try 
            { 
                CurrentUser.ChangeProfilePhoto(imageBytes); 
                LoadUserPlaylists(); 
                return Page(); 
            } 
            catch (Exception ex) 
            { 
                ErrorMessage = "An unexpected error has occurred please try again later"; 
                LoadUserPlaylists(); 
                return Page();
            }
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
