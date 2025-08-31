using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalBudgetPlanner.Models;
using PersonalBudgetPlanner.Services;

namespace PersonalBudgetPlanner.Pages
{
    public class IndexModel : PageModel
    {
        public List<Transaction> Transactions { get; set; } = new();
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
        public decimal Balance { get; set; }

        [BindProperty]
        public Transaction Input { get; set; } = new Transaction { Date = DateTime.Today };

        public IActionResult OnGet()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToPage("/Login");

            Transactions = BudgetService.GetTransactions(username);
            var s = BudgetService.Summary(username);
            Income = s.income; Expense = s.expense; Balance = s.balance;
            return Page();
        }

        public IActionResult OnPost()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToPage("/Login");

            Input.Username = username;
            if (Input.Date == default) Input.Date = DateTime.Today;
            BudgetService.SaveTransaction(Input);
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(Guid id)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToPage("/Login");

            BudgetService.DeleteTransaction(username, id);
            return RedirectToPage();
        }
    }
}
