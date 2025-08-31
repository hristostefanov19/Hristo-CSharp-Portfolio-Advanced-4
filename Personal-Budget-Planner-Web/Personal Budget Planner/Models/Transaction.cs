using System;

namespace PersonalBudgetPlanner.Models
{
    public class Transaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = "";
        public string Type { get; set; } = "Expense"; 
        public string Category { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Amount { get; set; } = 0m;
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
