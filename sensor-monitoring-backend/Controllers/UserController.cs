using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using sensor_monitoring_backend.Data;
using sensor_monitoring_backend.Domain.Entities;
using sensor_monitoring_backend.Domain.ViewModels;
using sensor_monitoring_backend.Infrastructure;
using sensor_monitoring_backend.Infrastructure.ApplicationSettings;
using sensor_monitoring_backend.Infrastructure.Attributes;

namespace sensor_monitoring_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        public UserController(DatabaseContext context,IOptions<Main> options)
        {
            _context = context;
            _mainSetting = options.Value;
        }

        private readonly DatabaseContext _context;
        private readonly Main _mainSetting;

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationModel>> Login(UserRequest userRequest)
        {
            if(userRequest == null)
            {
                return BadRequest();
            }

            else if (string.IsNullOrWhiteSpace(userRequest.UserName))
            {
                return BadRequest();
            }

            else if (string.IsNullOrWhiteSpace(userRequest.Password))
            {
                return BadRequest();
            }

            var user = await _context.Users.Where(currnet => currnet.UserName.ToLower() == userRequest.UserName.ToLower()).FirstOrDefaultAsync();
            if (user == null)
            {
                return BadRequest();
            }

            if(string.Compare(user.Password,userRequest.Password,false) != 0)
            {
                return BadRequest();
            }

            var token = JwtUtility.GenerateJwtToken(user, _mainSetting);

            AuthenticationModel authenticationModel = new AuthenticationModel(token, "", _mainSetting.TokenExpiresInMinutes,user.Role);

            return Ok(authenticationModel);
        }


        [HttpPost("register")]
        [Authorize("Admin")]
        public async Task<ActionResult> Register(UserRequest userRequest)
        {
            if (userRequest == null)
            {
                return BadRequest();
            }

            else if (string.IsNullOrWhiteSpace(userRequest.UserName) || string.IsNullOrWhiteSpace(userRequest.Password) || string.IsNullOrWhiteSpace(userRequest.FirstName) || string.IsNullOrWhiteSpace(userRequest.LastName) || string.IsNullOrWhiteSpace(userRequest.Role))
            {
                return BadRequest();
            }

            var user = new User()
            {
                UserName = userRequest.UserName,
                Password = userRequest.Password,
                FirstName = userRequest.FirstName,
                LastName = userRequest.LastName,
                Role = userRequest.Role
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok("User Create Successfully");
        }


        [HttpGet]
        [Authorize("Admin")]
        public async Task<ActionResult<List<User>>> GetAllUser()
        {
            var result = await _context.Users
                .Include(current => current.Devices)
                .ToListAsync();
            if (result == null)
            {
                return BadRequest("Not Found");
            }
            return Ok(result);
        }
    }
}
