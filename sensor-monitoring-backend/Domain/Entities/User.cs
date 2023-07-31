using sensor_monitoring_backend.Domain.Entity;

namespace sensor_monitoring_backend.Domain.Entities
{
    public class User
    {
        public User()
        {
            Code = Guid.NewGuid().ToString();
            RegisterDate = DateTime.UtcNow;
            Devices = new List<Device>();
        }

        public Guid Id { get; set; }
        public string Code { get; set; }
        public DateTime RegisterDate { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Role { get; set; }


        public virtual IList<Device> Devices { get; set; }

    }
}
