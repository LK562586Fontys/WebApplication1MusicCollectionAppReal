using LogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1MusicCollectionAppReal.Pages
{
    public class PlaylistScreen : PageModel
    {
        public static User CurrentUser { get; set; } = new LogicLayer.User { ID = 5 };
        public static LogicLayer.Playlist CurrentPlaylist { get; set; } = new LogicLayer.Playlist { ID = 5 };
        [BindProperty]
        public string NewName { get; set; }
        [BindProperty]
        public IFormFile NewPhoto { get; set; }
        public List<LogicLayer.Song> Songs { get; set; }

        public void OnGet()
        {
            LoadPlaylistSongs();
        }

        private void LoadPlaylistSongs()
        {
            Songs = CurrentPlaylist.LoadSongs();
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
    }
}
