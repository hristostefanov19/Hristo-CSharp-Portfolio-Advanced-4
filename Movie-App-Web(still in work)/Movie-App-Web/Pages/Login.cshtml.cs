using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieApp.Services;

namespace MovieApp.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty] public string Username { get; set; }
        [BindProperty] public string Password { get; set; }
        public string ErrorMessage { get; set; }

        public IActionResult OnPost()
        {
            var users = UserService.GetUsers();
            var user = users.FirstOrDefault(u => u.Username == Username && u.Password == Password);
            if (user == null)
            {
                ErrorMessage = "Invalid username or password.";
                return Page();
            }

            HttpContext.Session.SetString("Username", Username);
            return RedirectToPage("/Index");
        }
    }
}
