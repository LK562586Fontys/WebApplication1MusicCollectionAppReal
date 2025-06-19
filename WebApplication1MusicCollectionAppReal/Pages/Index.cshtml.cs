using Interfaces;
using LogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1MusicCollectionAppReal.Pages
{
    public class IndexModel : PageModel
    {
        private UserFactory _userService;
        private readonly ILogger<IndexModel> _logger;
        private User CurrentUser { get; set; }

        public IndexModel(ILogger<IndexModel> logger, UserFactory userService)
        {
            _logger = logger;
            _userService = userService;
        }
        public string TestResult { get; set; }
        public void OnGet()
        {
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                // Not logged in — redirect to login
                Response.Redirect("/Login");
                return;
            }

            // Set the current user
            CurrentUser = _userService.GetUserById((int)userId);
        }
    }
}
