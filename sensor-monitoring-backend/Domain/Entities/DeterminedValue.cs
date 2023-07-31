using sensor_monitoring_backend.Domain.Entities;

namespace sensor_monitoring_backend.Domain.Entity
{
    public class DeterminedValue
    {
        public DeterminedValue()
        {
            Code = Guid.NewGuid().ToString();
            RegisterDate = DateTime.UtcNow;
        }
        public Guid Id { get; set; }
        public string Code { get; set; }
        public DateTime RegisterDate { get; set; }


        public double Value { get; set; }
        //public double? RawValue { get; set; }

        //public double? FinalValue { get; set; }

        public DateTime? DateTimeDetermined { get; set; }


        public virtual PacketDecoder? PacketDecoder { get; set; }

        public Guid? PacketDecoderId { get; set; }

        //public virtual Sensor? Sensor { get; set; }

        //public Guid? SensorId { get; set; }
    }
}
