using sensor_monitoring_backend.Domain.Entity;

namespace sensor_monitoring_backend.Domain.Entities
{
    public class Device
    {
        public Device()
        {
            Code = Guid.NewGuid().ToString();
            RegisterDate = DateTime.UtcNow;
            Sensors = new List<Sensor>();
        }

        public Guid Id { get; set; }
        public string Code { get; set; }
        public DateTime RegisterDate { get; set; }
        public string? DeviceModel { get; set; }
        public string? PhoneNumber { get; set; }
        public string? IMEIModem { get; set; }
        public string? SimCardCharge { get; set; }

        public string? NickName { get; set; }

        public Double? LatPosition { get; set; }
        public Double? LngPosition { get; set; }

        public virtual IList<Sensor> Sensors { get; set; }

        public virtual User User { get; set; }

        public Guid? UserId { get; set; }
    }
}
