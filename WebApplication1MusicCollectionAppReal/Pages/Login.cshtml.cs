using LogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1MusicCollectionAppReal.Pages
{
    public class Login : PageModel
    {
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
			return null;
		}
	}
}
