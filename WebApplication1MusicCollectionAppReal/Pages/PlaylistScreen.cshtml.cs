using Interfaces;
using LogicLayer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1MusicCollectionAppReal.Pages.ViewModels;

namespace WebApplication1MusicCollectionAppReal.Pages
{
    public class PlaylistScreen : PageModel
    {
        private readonly UserFactory _userService;
        private readonly PlaylistFactory _playlistService;
        private readonly IUserRepository _userRepository;
        private UserMapper _userMapper;
        public PlaylistViewModel viewModel { get; set; }
        public static User CurrentUser { get; set; }
        public string ErrorMessage { get; private set; }
        public static LogicLayer.Playlist CurrentPlaylist { get; set; }
        [BindProperty]
        public string NewName { get; set; }
        [BindProperty]
        public IFormFile NewPhoto { get; set; }
        public List<LogicLayer.Song> Songs { get; set; }

        public PlaylistScreen(UserFactory userService, PlaylistFactory playlistService, IUserRepository userRepository) 
        {
            _userService = userService;
            _playlistService = playlistService;
            _userRepository = userRepository;
            _userMapper = new UserMapper();
        }
        public IActionResult OnGet(int id)
        {
			int? userId = HttpContext.Session.GetInt32("UserID");

			if (userId == null)
			{
				// Not logged in — redirect to login
				return RedirectToPage("/Login");
            }

            // Set the current user
			CurrentUser = _userService.GetUserById((int)userId);
            CurrentPlaylist = _playlistService.GetPlaylistById(id);
            if (CurrentPlaylist == null)
            {
                return RedirectToPage("/Error", new { message = "Playlist not found" });
            }
			LoadPlaylistSongs();
            return Page();
        }

        private void LoadPlaylistSongs()
        {

            var sortField = HttpContext.Session.GetString("SortField");
            var sortOrder = HttpContext.Session.GetString("SortOrder");

            CurrentPlaylist.UpdatePlaylistList(sortField, sortOrder);

            var creatorUser = _userMapper.FromDataModel(CurrentPlaylist.Creator);
            viewModel = new PlaylistViewModel
            {
                ID = CurrentPlaylist.ID,
                DateAdded = CurrentPlaylist.DateAdded,
                Photo = CurrentPlaylist.Photo,
                Name = CurrentPlaylist.Name,
                Creator = creatorUser,
                Base64Photo = CurrentPlaylist.Base64Photo,
                playlistSongs = CurrentPlaylist.PlaylistSongs,
            };
            if (CurrentPlaylist.Photo != null)
            {
                CurrentPlaylist.Base64Photo = Convert.ToBase64String(CurrentPlaylist.Photo);
                viewModel.Base64Photo = Convert.ToBase64String(CurrentPlaylist.Photo);
            }
        }
        public IActionResult OnPostChangePlaylistName()
        {
            try
            {
                CurrentPlaylist.ChangePlaylistName(NewName);
                return RedirectToPage(new { id = CurrentPlaylist.ID });
            }
            catch (ArgumentException ex) 
            {
                ErrorMessage = ex.Message;
                LoadPlaylistSongs();
                return Page();
            }
            catch (Exception) 
            {
				ErrorMessage = "An unexpected error has occurred. Please try again later";
				return RedirectToPage("/Error", new { message = ErrorMessage });
			}
        }
        public async Task<IActionResult> OnPostChangePlaylistPhoto()
        {
            using var memoryStream = new MemoryStream();
            await NewPhoto.CopyToAsync(memoryStream);
            byte[] imageBytes = memoryStream.ToArray();
            try
            {
                CurrentPlaylist.ChangePlaylistPicture(imageBytes);
                return RedirectToPage(new { id = CurrentPlaylist.ID });
            }
            catch (Exception) 
            {
				ErrorMessage = "An unexpected error has occurred. Please try again later";
				return RedirectToPage("/Error", new { message = ErrorMessage });
			}
        }
        public IActionResult OnPostDeletePlaylist() 
        {
            CurrentPlaylist.DeletePlaylist(CurrentPlaylist.ID);
            return RedirectToPage("/Account");
        }
        
    }
}
