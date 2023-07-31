using sensor_monitoring_backend.Domain.Entity;
using sensor_monitoring_backend.Domain.Enums;

namespace sensor_monitoring_backend.Domain.Entities
{
    public class PacketDecoder
    {
        public PacketDecoder()
        {
            Code = Guid.NewGuid().ToString();
            RegisterDate = DateTime.UtcNow;
            DeterminedValues = new List<DeterminedValue>();
        }
        public Guid Id { get; set; }
        public string Code { get; set; }
        public DateTime RegisterDate { get; set; }

        public List<Double> ByteNumbers { get; set; }

        public Numerical KindProperty { get; set; }

        public string NameProperty { get; set; }

        public int StartByte { get; set; }


        public virtual Sensor PacketSensor { get; set; }

        public Guid? SensorId { get; set; }

        public virtual IList<DeterminedValue> DeterminedValues { get; set; }

    }
}
