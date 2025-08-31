using PersonalBudgetPlanner.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PersonalBudgetPlanner.Services
{
    public static class UserService
    {
        private static readonly string FilePath = Path.Combine("App_Data", "budget_users.json");

        public static List<User> GetUsers()
        {
            if (!File.Exists(FilePath)) return new List<User>();
            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        public static void SaveUsers(List<User> users)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
            var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
}
