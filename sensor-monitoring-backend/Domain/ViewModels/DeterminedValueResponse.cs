using sensor_monitoring_backend.Domain.Entity;
using sensor_monitoring_backend.Domain.Enums;

namespace sensor_monitoring_backend.Domain.ViewModels
{
    public class DeterminedValueResponse
    {
        public List<DeterminedValue>? determinedValues { get; set; }

        public int totalCount { get; set; }
    }
}
