namespace sensor_monitoring_backend.Infrastructure.ApplicationSettings
{
    public class Main : object
    {
        public Main()
        {

        }

        public string SecretKey { get; set; }

        public int TokenExpiresInMinutes { get; set; }
    }
}
