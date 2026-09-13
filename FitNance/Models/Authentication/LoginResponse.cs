namespace FitNance.Models.Authentication
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public bool IsProfileComplete { get; set; }
    }
}
