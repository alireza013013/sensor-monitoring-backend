namespace sensor_monitoring_backend.Domain.ViewModels
{
    public class AuthRequest
    {
        public AuthRequest(string token, int tokenExpiresInMinutes)
        {
            Token = token;
            TokenExpiresInMinutes = tokenExpiresInMinutes;
        }

        public string Token { get; set; }
        public int TokenExpiresInMinutes { get; set; }
    }
}
