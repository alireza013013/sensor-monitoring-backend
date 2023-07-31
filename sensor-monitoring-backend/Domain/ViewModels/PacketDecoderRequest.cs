using sensor_monitoring_backend.Domain.Entity;
using sensor_monitoring_backend.Domain.Enums;

namespace sensor_monitoring_backend.Domain.ViewModels
{
    public class PacketDecoderRequest
    {
        public List<Double> ByteNumbers { get; set; }

        public Numerical KindProperty { get; set; }

        public string NameProperty { get; set; }

        public int StartByte { get; set; }
    }
}
