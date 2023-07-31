using sensor_monitoring_backend.Domain.Entity;

namespace sensor_monitoring_backend.Domain.ViewModels
{
    public class DeviceRequest
    {
        public string? DeviceModel { get; set; }
        public string? PhoneNumber { get; set; }
        public string? IMEIModem { get; set; }
        public string? SimCardCharge { get; set; }

        public string? NickName { get; set; }

        public double? LatPosition { get; set; }
        public double? LngPosition { get; set; }

        public List<SensorRequest>? Sensors { get; set; }

        public Guid? UserId { get; set; }
    }
}
