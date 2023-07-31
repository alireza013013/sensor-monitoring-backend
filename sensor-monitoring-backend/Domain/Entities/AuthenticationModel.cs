namespace sensor_monitoring_backend.Domain.Entities
{
    public class AuthenticationModel
    {

        public AuthenticationModel(string token,string refreshToken,int tokenExpiresInMinutes, string role)
        {
            Token = token; 
            RefreshToken = refreshToken;
            TokenExpiresInMinutes = tokenExpiresInMinutes;
            Role = role;
        }

        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public int TokenExpiresInMinutes { get; set; }

        public string Role { get; set; }
    }
}
