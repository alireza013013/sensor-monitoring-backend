using sensor_monitoring_backend.Domain.Entity;
using sensor_monitoring_backend.Domain.Enums;

namespace sensor_monitoring_backend.Domain.Entities
{
    public class TemplateSensor
    {
        public TemplateSensor()
        {
            Code = Guid.NewGuid().ToString();
            RegisterDate = DateTime.UtcNow;
            Sensors = new List<Sensor>();
        }

        public Guid Id { get; set; }
        public string Code { get; set; }
        public DateTime RegisterDate { get; set; }
      
        public string? NickName { get; set; }

        public SensorType? Kind { get; set; }

        public string? UnitMeasurement { get; set; }

        public virtual IList<Sensor> Sensors { get; set; }

    }
}
