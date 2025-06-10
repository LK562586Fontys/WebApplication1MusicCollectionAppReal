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

        public List<LogicLayer.Song> Songs { get; set; }

        public void OnGet()
        {
            Songs = AllSongs.SearchSongs(SearchTerm);
        }
    }
}
