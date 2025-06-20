using Interfaces;
using LogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1MusicCollectionAppReal.Pages
{
    public class Login : PageModel
    {
		private readonly UserFactory _userFactory;
		private static User CurrentUser { get; set; }
		public string ErrorMessage { get; private set; }
		[BindProperty]
        public string EmailAddress { get; set; }
        [BindProperty]
        public string Password { get; set; }
        public void OnGet()
        {
			HttpContext.Session.Clear();
		}
        public Login(UserFactory userFactory)
        {
            _userFactory = userFactory;
        }
        public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			
			int? userId = await _userFactory.VerifyLoginAndReturnUserId(EmailAddress, Password);

			if (userId == null)
			{
				ErrorMessage = "Invalid email address or password.";
				return Page();
			}

			// Store user ID in session
			HttpContext.Session.SetInt32("UserID", userId.Value);

			return RedirectToPage("/Index");
		}
	}
}
