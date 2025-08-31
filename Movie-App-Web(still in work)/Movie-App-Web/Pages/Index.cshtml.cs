using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieApp.Models;
using MovieApp.Services;
using System.Net.Http;
using System.Text.Json;

namespace MovieApp.Pages
{
    public class IndexModel : PageModel
    {
        private const string OmdbApiKey = "561483b5"; // твоя OMDb API ключ

        public List<string> Favorites { get; set; } = new();

        public void OnGet()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username)) return;

            var users = UserService.GetUsers();
            var user = users.FirstOrDefault(u => u.Username == username);
            if (user != null) Favorites = user.FavoriteMovies;
        }

        public async Task<JsonResult> OnGetSearchAsync(string query)
        {
            if (string.IsNullOrEmpty(query))
                return new JsonResult(new { Search = Array.Empty<string>() });

            using var client = new HttpClient();
            string url = $"http://www.omdbapi.com/?s={query}&apikey={OmdbApiKey}";
            string response = await client.GetStringAsync(url);

            var json = JsonSerializer.Deserialize<JsonElement>(response);
            return new JsonResult(json);
        }

        public JsonResult OnPostAddFavorite([FromBody] MovieRequest data)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return new JsonResult(new { success = false });

            var users = UserService.GetUsers();
            var user = users.FirstOrDefault(u => u.Username == username);

            if (user != null && !user.FavoriteMovies.Contains(data.Title))
            {
                Console.WriteLine($"Adding favorite: {data.Title} for {username}");
                user.FavoriteMovies.Add(data.Title);
                UserService.SaveUsers(users);
            }

            return new JsonResult(new { success = true });
        }

        public JsonResult OnPostRemoveFavorite([FromBody] MovieRequest data)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return new JsonResult(new { success = false });

            var users = UserService.GetUsers();
            var user = users.FirstOrDefault(u => u.Username == username);

            if (user != null && user.FavoriteMovies.Contains(data.Title))
            {
                Console.WriteLine($"Removing favorite: {data.Title} for {username}");
                user.FavoriteMovies.Remove(data.Title);
                UserService.SaveUsers(users);
            }

            return new JsonResult(new { success = true });
        }
    }

    public class MovieRequest
    {
        public string Title { get; set; }
    }
}
