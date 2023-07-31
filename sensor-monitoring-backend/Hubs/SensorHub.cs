using Microsoft.AspNetCore.SignalR;
using sensor_monitoring_backend.Domain.Entity;

namespace sensor_monitoring_backend.Hubs
{
    public class SensorHub : Hub
    {
        public async Task SendDeterminedValue(DeterminedValue determinedValue)
        {
            await Clients.All.SendAsync("ReciveDeterminedValue", determinedValue);
        }
    }
}
