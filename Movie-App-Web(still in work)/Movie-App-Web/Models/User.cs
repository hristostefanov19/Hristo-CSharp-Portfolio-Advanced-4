namespace MovieApp.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public List<string> FavoriteMovies { get; set; } = new();
    }
}