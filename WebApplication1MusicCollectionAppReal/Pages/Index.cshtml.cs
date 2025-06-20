using Interfaces;
using LogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1MusicCollectionAppReal.Pages
{
    public class IndexModel : PageModel
    {
		private readonly SongFactory _songFactory;
		public static LogicLayer.Song AllSongs { get; set; }
		[BindProperty(SupportsGet = true)]
		public string SearchTerm { get; set; }
		private UserFactory _userFactory;
        private readonly ILogger<IndexModel> _logger;
        private User CurrentUser { get; set; }
		public List<LogicLayer.Song> Songs { get; set; }

		public IndexModel(ILogger<IndexModel> logger, UserFactory userFactory, SongFactory songFactory)
        {
            _logger = logger;
            _userFactory = userFactory;
			_songFactory = songFactory;
		}
        public string TestResult { get; set; }
        public IActionResult OnGet()
        {
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
				// Not logged in — redirect to login
				return RedirectToPage("/Login");
			}

            // Set the current user
            CurrentUser = _userFactory.GetUserById((int)userId);
			if (AllSongs == null)
			{
				AllSongs = (LogicLayer.Song)_songFactory.GetSongById(1);
			}
			Songs = AllSongs.SearchSongs(SearchTerm);
			return Page();
		}
    }
}
