namespace TokenGenerationApplication.Models
{
    public class LoginResponse
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string Accesstoken { get; set; }

        public int ExpiresIn { get; set; }
    }
}
