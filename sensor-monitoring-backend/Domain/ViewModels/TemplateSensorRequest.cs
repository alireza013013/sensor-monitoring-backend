using sensor_monitoring_backend.Domain.Enums;

namespace sensor_monitoring_backend.Domain.ViewModels
{
    public class TemplateSensorRequest
    {
        public string? NickName { get; set; }

        public SensorType? Kind { get; set; }

        public string? UnitMeasurement { get; set; }
    }
}
