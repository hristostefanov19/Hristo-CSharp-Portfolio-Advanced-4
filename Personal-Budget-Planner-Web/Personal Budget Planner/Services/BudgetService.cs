using PersonalBudgetPlanner.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace PersonalBudgetPlanner.Services
{
    public static class BudgetService
    {
        private static readonly string FilePath = Path.Combine("App_Data", "transactions.json");

        public static List<Transaction> GetTransactions(string username)
        {
            if (!File.Exists(FilePath)) return new List<Transaction>();
            var json = File.ReadAllText(FilePath);
            var all = JsonSerializer.Deserialize<List<Transaction>>(json) ?? new List<Transaction>();
            return all.Where(t => t.Username == username).OrderByDescending(t => t.Date).ToList();
        }

        public static void SaveTransaction(Transaction t)
        {
            var all = GetAll();
            all.Add(t);
            SaveAll(all);
        }

        public static void DeleteTransaction(string username, System.Guid id)
        {
            var all = GetAll();
            all.RemoveAll(t => t.Username == username && t.Id == id);
            SaveAll(all);
        }

        public static (decimal income, decimal expense, decimal balance) Summary(string username)
        {
            var tx = GetTransactions(username);
            decimal income = tx.Where(t => t.Type == "Income").Sum(t => t.Amount);
            decimal expense = tx.Where(t => t.Type == "Expense").Sum(t => t.Amount);
            return (income, expense, income - expense);
        }

        private static List<Transaction> GetAll()
        {
            if (!File.Exists(FilePath)) return new List<Transaction>();
            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Transaction>>(json) ?? new List<Transaction>();
        }

        private static void SaveAll(List<Transaction> all)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
            var json = JsonSerializer.Serialize(all, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
}
