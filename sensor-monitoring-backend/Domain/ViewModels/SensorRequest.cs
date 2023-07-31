using sensor_monitoring_backend.Domain.Entity;
using sensor_monitoring_backend.Domain.Enums;

namespace sensor_monitoring_backend.Domain.ViewModels
{
    public class SensorRequest
    {

        public Guid? TemplateSensorId { get; set; }

        public PortNames? PortName { get; set; }

        public PortTypes? PortKind { get; set; }

        public List<PacketDecoderRequest>? PacketDecoderRequests { get; set; }
    }
}
