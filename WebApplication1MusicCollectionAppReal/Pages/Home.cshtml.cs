using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1MusicCollectionAppReal.Pages
{
    public class Home : PageModel
    {
        public LogicLayer.Song AllSongs { get; set; } = new LogicLayer.Song();
        public IList<Song> Songs { get; set; }
        [BindProperty]
        public string SearchTerm { get; set; }
        

        public async Task OnGetAsync()
        {
            AllSongs.GetAllSongs();
        }
    }
}
