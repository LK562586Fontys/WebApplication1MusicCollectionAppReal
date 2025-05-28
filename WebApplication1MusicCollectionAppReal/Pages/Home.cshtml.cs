using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1MusicCollectionAppReal.Pages
{
    public class Home : PageModel
    {
        public LogicLayer.Song AllSongs { get; set; } = new LogicLayer.Song();
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public List<LogicLayer.Song> Songs { get; set; }

        public void OnGet()
        {
            Songs = AllSongs.SearchSongs(SearchTerm);
        }
    }
}
