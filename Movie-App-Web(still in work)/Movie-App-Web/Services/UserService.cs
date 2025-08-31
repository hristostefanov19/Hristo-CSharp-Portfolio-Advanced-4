using MovieApp.Models;
using System.Text.Json;

namespace MovieApp.Services
{
    public static class UserService
    {
        private static readonly string filePath =
    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "users.json");


        public static List<User> GetUsers()
        {
            if (!File.Exists(filePath)) return new List<User>();
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        public static void SaveUsers(List<User> users)
        {
            Directory.CreateDirectory("App_Data");
            string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}
