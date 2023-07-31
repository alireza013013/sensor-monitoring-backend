using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sensor_monitoring_backend.Data;
using sensor_monitoring_backend.Domain.Entity;
using sensor_monitoring_backend.Domain.ViewModels;
using sensor_monitoring_backend.Infrastructure.Attributes;
using System.Text;

namespace sensor_monitoring_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SensorController : ControllerBase
    {
        public SensorController(DatabaseContext context)
        {
            _context = context;
        }

        private readonly DatabaseContext _context;

        [HttpGet]
        [Authorize("Admin")]
        public async Task<ActionResult<List<Sensor>>> Get()
        {
            var result = await _context.Sensors.ToListAsync();
            return Ok(result);
        }


        [HttpPost("determinedValues")]
        [Authorize]
        public async Task<ActionResult<List<DeterminedValue>>> GetDetermineValue(PaginationRequest pagination)
        {
            var determinedValues = new List<DeterminedValue>();
            int totalCount;
            DeterminedValueResponse result = new DeterminedValueResponse();
            if (pagination.SearchValue != null)
            {
                determinedValues = await _context.DeterminedValues
                .Where(current => current.PacketDecoderId == pagination.Id)
                .Where(current => current.Value == pagination.SearchValue)
                .OrderByDescending(current => current.RegisterDate)
                .Skip((pagination.PageNumber - 1) * pagination.CountPage)
                .Take(pagination.CountPage)
                .ToListAsync();
                totalCount = await _context.DeterminedValues
                   .Where(current => current.PacketDecoderId == pagination.Id)
                   .Where(current => current.Value == pagination.SearchValue)
                   .CountAsync();
                result = new DeterminedValueResponse()
                {
                    determinedValues = determinedValues,
                    totalCount = totalCount
                };
            }
            else
            {
                determinedValues = await _context.DeterminedValues
                 .Where(current => current.PacketDecoderId == pagination.Id)
                 .OrderByDescending(current => current.RegisterDate)
                 .Skip((pagination.PageNumber - 1) * pagination.CountPage)
                 .Take(pagination.CountPage)
                 .ToListAsync();
               totalCount = await _context.DeterminedValues
                  .Where(current => current.PacketDecoderId == pagination.Id)
                  .CountAsync();
               result = new DeterminedValueResponse()
               {
                  determinedValues = determinedValues,
                  totalCount = totalCount
               };
            }
            if (determinedValues == null)
            {
                return BadRequest("Not Found");
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Sensor>> GetSensor(Guid id)
        {
            var result = await _context.Sensors
                .Include(current => current.TemplateSensor)
                .Include(current => current.PacketDecoders)
                .FirstOrDefaultAsync(current => current.Id == id);
            if (result == null)
            {
                return BadRequest("Not Found");
            }
            return Ok(result);
        }


        [HttpGet("GenerateExel/{id}")]
        [Authorize]
        public async Task<ActionResult> GenerateExelFile(Guid id)
        {
            var result = await _context.DeterminedValues.Where(item => item.PacketDecoderId == id).ToListAsync();
            if (result == null)
            {
                return BadRequest("Not Found");
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<table border=`" + "1px" + "`b>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td><b><font face=Arial Narrow size=3>Date Detemined</font></b></td>");
            stringBuilder.Append("<td><b><font face=Arial Narrow size=3>Final Value</font></b></td>");
            stringBuilder.Append("</tr>");
            foreach (DeterminedValue determinedValue in result)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + determinedValue.DateTimeDetermined.ToString() + "</font></td>");
                stringBuilder.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + determinedValue.Value.ToString() + "</font></td>");
                stringBuilder.Append("</tr>");
            }
            stringBuilder.Append("</table>");
            HttpContext.Response.Headers.Add("content-disposition", "attachment; filename=Information" + DateTime.Now.Year.ToString() + ".xls");
            this.Response.ContentType = "application/vnd.ms-excel";
            byte[] temp = System.Text.Encoding.UTF8.GetBytes(stringBuilder.ToString());
            return File(temp, "application/vnd.ms-excel");
        }
    }
}
