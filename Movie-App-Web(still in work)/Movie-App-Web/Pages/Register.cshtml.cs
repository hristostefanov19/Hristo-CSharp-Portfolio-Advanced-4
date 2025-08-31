using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieApp.Models;
using MovieApp.Services;

namespace MovieApp.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty] public string Username { get; set; }
        [BindProperty] public string Password { get; set; }
        public string ErrorMessage { get; set; }

        public IActionResult OnPost()
        {
            var users = UserService.GetUsers();
            if (users.Any(u => u.Username == Username))
            {
                ErrorMessage = "Username already exists.";
                return Page();
            }

            users.Add(new User { Username = Username, Password = Password });
            UserService.SaveUsers(users);

            HttpContext.Session.SetString("Username", Username);
            return RedirectToPage("/Index");
        }
    }
}
