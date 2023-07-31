namespace sensor_monitoring_backend.Domain.ViewModels
{
    public class PaginationRequest
    {
        public Guid? Id { get; set; }

        public int PageNumber { get; set; }

        public int CountPage { get; set; }

        public double? SearchValue { get; set; }
    }
}
