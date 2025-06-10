using Interfaces;
using LogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1MusicCollectionAppReal.Pages
{
    public class PlaylistScreen : PageModel
    {
        private readonly IUserService _userService;
        private readonly IPlaylistService _playlistService;
        public static User CurrentUser { get; set; }
        public static LogicLayer.Playlist CurrentPlaylist { get; set; }
        [BindProperty]
        public string NewName { get; set; }
        [BindProperty]
        public IFormFile NewPhoto { get; set; }
        public List<LogicLayer.Song> Songs { get; set; }

        public PlaylistScreen(IUserService userService, IPlaylistService playlistService) 
        {
            _userService = userService;
            _playlistService = playlistService;
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
            CurrentPlaylist = (LogicLayer.Playlist)_playlistService.GetPlaylistById(id);
			CurrentPlaylist.GetSpecificPlaylist(id);
			LoadPlaylistSongs();
        }

        private void LoadPlaylistSongs()
        {
            var sortField = HttpContext.Session.GetString("SortField");
            var sortOrder = HttpContext.Session.GetString("SortOrder");

            CurrentPlaylist.UpdatePlaylistList(sortField, sortOrder);
        }
        public IActionResult OnPostChangePlaylistName()
        {
            CurrentPlaylist.ChangePlaylistName(NewName);
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostChangePlaylistPhoto()
        {
            using var memoryStream = new MemoryStream();
            await NewPhoto.CopyToAsync(memoryStream);
            byte[] imageBytes = memoryStream.ToArray();
            CurrentPlaylist.ChangePlaylistPicture(imageBytes);
            return RedirectToPage();
        }
        public IActionResult OnPostDeletePlaylist() 
        {
            CurrentPlaylist.DeletePlaylist(CurrentPlaylist.ID);
            return RedirectToPage("/Account");
        }
        public IActionResult OnPostSortingBy() 
        {
            var sort = Request.Form["sort"].ToString();

            if (!string.IsNullOrEmpty(sort))
            {
                var parts = sort.Split('_');
                if (parts.Length == 2)
                {
                    var sortField = parts[0];
                    var sortOrder = parts[1];

                    HttpContext.Session.SetString("SortField", sortField);
                    HttpContext.Session.SetString("SortOrder", sortOrder);

                    if (sortField == "random")
                    {
                        var seed = Guid.NewGuid().GetHashCode();
                        HttpContext.Session.SetString("ShuffleSeed", seed.ToString());
                    }
                    else
                    {
                        HttpContext.Session.Remove("ShuffleSeed");
                    }
                }
            }

            return RedirectToPage();
        }
    }
}
