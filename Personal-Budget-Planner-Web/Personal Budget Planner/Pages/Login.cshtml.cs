using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalBudgetPlanner.Services;
using System.Text.Json;

namespace PersonalBudgetPlanner.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty] public string Username { get; set; } = "";
        [BindProperty] public string Password { get; set; } = "";
        public string ErrorMessage { get; set; } = "";

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            var users = UserService.GetUsers();
            var user = users.FirstOrDefault(u => u.Username == Username && u.Password == Password);
            if (user == null)
            {
                ErrorMessage = "Invalid username or password!";
                return Page();
            }

            HttpContext.Session.SetString("Username", user.Username);
            return RedirectToPage("/Index");
        }
    }
}
