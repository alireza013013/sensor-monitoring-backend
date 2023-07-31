using Microsoft.AspNetCore.Mvc;
using sensor_monitoring_backend.Data;
using sensor_monitoring_backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using sensor_monitoring_backend.Domain.ViewModels;
using sensor_monitoring_backend.Infrastructure.Attributes;

namespace sensor_monitoring_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TemplateSensorController : ControllerBase
    {
        public TemplateSensorController(DatabaseContext context)
        {
            _context = context;
        }

        private readonly DatabaseContext _context;


        [HttpPost]
        [Authorize("Admin")]
        public async Task<IActionResult> Create(TemplateSensorRequest templateSensorRequest)
        {
            TemplateSensor templateSensor = new TemplateSensor()
            {
                NickName = templateSensorRequest.NickName,
                UnitMeasurement = templateSensorRequest.UnitMeasurement,
                Kind = templateSensorRequest.Kind,
            };
            await _context.TemplateSensors.AddAsync(templateSensor);
            await _context.SaveChangesAsync();
            return Ok("Create SuccessFully");
        }


        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<TemplateSensor>>> Get()
        {
            var result = await _context.TemplateSensors.ToListAsync();
            return Ok(result);
        }
    }
}
