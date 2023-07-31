using sensor_monitoring_backend.Domain.Entities;
using sensor_monitoring_backend.Domain.Enums;

namespace sensor_monitoring_backend.Domain.Entity
{
    public class Sensor
    {
        public Sensor()
        {
            Code = Guid.NewGuid().ToString();
            RegisterDate = DateTime.UtcNow;
            //DeterminedValues = new List<DeterminedValue>();
        }

        public Guid Id { get; set; }
        public string Code { get; set; }
        public DateTime RegisterDate { get; set; }

        public PortNames? PortName { get; set; }

        public PortTypes? PortKind { get; set; }

        public virtual Device Device { get; set; }

        public Guid? DeviceId { get; set; }

        public virtual TemplateSensor TemplateSensor { get; set; }

        public Guid? TemplateSensorId { get; set; }

        //public virtual IList<DeterminedValue> DeterminedValues { get; set; }


        public virtual IList<PacketDecoder> PacketDecoders { get; set; }
    }
}
