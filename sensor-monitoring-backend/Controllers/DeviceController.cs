using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using sensor_monitoring_backend.Data;
using sensor_monitoring_backend.Domain.Entities;
using sensor_monitoring_backend.Domain.Entity;
using sensor_monitoring_backend.Domain.ViewModels;
using sensor_monitoring_backend.Infrastructure.Attributes;
using System.Numerics;

namespace sensor_monitoring_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeviceController : ControllerBase
    {

        public DeviceController(DatabaseContext context)
        {
            _context = context;
        }

        private readonly DatabaseContext _context;


        [HttpPost("Admin")]
        [Authorize("Admin")]
        public async Task<IActionResult> Create(DeviceRequest deviceRequest)
        {
            Device device = new Device()
            {
                DeviceModel = deviceRequest.DeviceModel,
                NickName = deviceRequest.NickName,
                IMEIModem = deviceRequest.IMEIModem,
                PhoneNumber = deviceRequest.PhoneNumber,
                SimCardCharge = deviceRequest.SimCardCharge,
                LatPosition = deviceRequest.LatPosition,
                LngPosition = deviceRequest.LngPosition,
                UserId = deviceRequest.UserId,
            };
            await _context.Devices.AddAsync(device);
            foreach (SensorRequest sensorRequest in deviceRequest.Sensors!)
            {
                var templateSensor = await _context.TemplateSensors.FindAsync(sensorRequest.TemplateSensorId);
                if (templateSensor != null)
                {
                    Sensor sensor = new Sensor()
                    {
                        PortKind = sensorRequest.PortKind,
                        PortName = sensorRequest.PortName,
                        Device = device,
                        DeviceId = device.Id,
                        TemplateSensorId = templateSensor.Id,
                        TemplateSensor = templateSensor
                    };
                    device.Sensors.Add(sensor);
                    await _context.Sensors.AddAsync(sensor);
                    foreach (PacketDecoderRequest packetDecoderRequest in sensorRequest.PacketDecoderRequests)
                    {
                        PacketDecoder packetDecoder = new PacketDecoder()
                        {
                            ByteNumbers = packetDecoderRequest.ByteNumbers,
                            KindProperty = packetDecoderRequest.KindProperty,
                            NameProperty = packetDecoderRequest.NameProperty,
                            StartByte = packetDecoderRequest.StartByte,
                            SensorId = sensor.Id,
                            PacketSensor = sensor,
                        };
                        //sensor.PacketDecoders.Add(packetDecoder);
                        await _context.PacketDecoders.AddAsync(packetDecoder);
                    }
                }
            }
            await _context.SaveChangesAsync();
            return Ok("Device successfully created");
        }



        [HttpGet("Admin")]
        [Authorize("Admin")]
        public async Task<ActionResult<List<Device>>> GetAll()
        {
            var result = await _context.Devices.Include(current => current.Sensors).Include(current => current.User).ToListAsync();
            if (result == null)
            {
                return BadRequest("Not Found");
            }
            return Ok(result);
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Device>> GetDevice(Guid id)
        {
            var result = await _context.Devices
                .Include(current => current.Sensors)
                .ThenInclude(current => current.TemplateSensor)
                .Include(current => current.User)
                .FirstOrDefaultAsync(current => current.Id == id);
            if (result == null)
            {
                return BadRequest("Not Found");
            }
            return Ok(result);
        }



        [HttpGet]
        [Authorize("User")]
        public async Task<ActionResult<List<Device>>> GetDevices()
        {
            User? userFromContext = HttpContext?.Items["User"] as User;
            if (userFromContext != null)
            {
                var result = await _context.Devices
                    .Where(current => current.UserId == userFromContext.Id)
                    .Include(current => current.Sensors)
                    .Include(current => current.User)
                    .ToListAsync();
                if (result == null)
                {
                    return BadRequest("Not Found");
                }
                return Ok(result);
            }

            return BadRequest();
        }
    }
}
