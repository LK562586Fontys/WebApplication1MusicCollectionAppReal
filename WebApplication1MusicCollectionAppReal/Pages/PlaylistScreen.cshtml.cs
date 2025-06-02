using LogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1MusicCollectionAppReal.Pages
{
    public class PlaylistScreen : PageModel
    {
        public static User CurrentUser { get; set; }
        public static LogicLayer.Playlist CurrentPlaylist { get; set; } = new LogicLayer.Playlist { ID = 6 };
        [BindProperty]
        public string NewName { get; set; }
        [BindProperty]
        public IFormFile NewPhoto { get; set; }
        public List<LogicLayer.Song> Songs { get; set; }

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
			CurrentPlaylist = new LogicLayer.Playlist { ID = id };
			CurrentPlaylist.LoadSpecificPlaylist();
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
            return RedirectToPage();
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
