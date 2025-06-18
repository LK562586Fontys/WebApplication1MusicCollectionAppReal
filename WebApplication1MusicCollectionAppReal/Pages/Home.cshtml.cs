using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1MusicCollectionAppReal.Pages
{
    public class Home : PageModel
    {
        private readonly ISongService _songService;
        public static LogicLayer.Song AllSongs { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public Home(ISongService songService) 
        {
            _songService = songService;
        }
        public List<LogicLayer.Song> Songs { get; set; }

        public IActionResult OnGet()
        {
            if (AllSongs == null)
            {
                AllSongs = (LogicLayer.Song)_songService.GetSongById(1);
            }
            Songs = AllSongs.SearchSongs(SearchTerm);
            return Page();
        }
    }
}
