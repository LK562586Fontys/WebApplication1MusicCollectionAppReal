using LogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1MusicCollectionAppReal.Pages
{
    public class Login : PageModel
    {
		private User _userService { get; set; } = new User();
		[BindProperty]
        public string EmailAddress { get; set; }
        [BindProperty]
        public string Password { get; set; }
        public void OnGet()
        {
        }
		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			int? userId = await _userService.VerifyLoginAndReturnUserId(EmailAddress, Password);

			if (userId == null)
			{
				ModelState.AddModelError(string.Empty, "Invalid email or password.");
				return Page();
			}

			// Store user ID in session
			HttpContext.Session.SetInt32("UserID", userId.Value);

			return RedirectToPage("/Index");
		}
	}
}
