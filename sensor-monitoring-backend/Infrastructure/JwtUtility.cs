using Microsoft.IdentityModel.Tokens;
using sensor_monitoring_backend.Data;
using sensor_monitoring_backend.Domain.Entities;
using sensor_monitoring_backend.Infrastructure.ApplicationSettings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace sensor_monitoring_backend.Infrastructure
{
    public class JwtUtility
    {

        static JwtUtility()
        {
        }

        public static string GenerateJwtToken(User user, Main mainSetting)
        {
            byte[] key = Encoding.ASCII.GetBytes(mainSetting.SecretKey);
            var symmetricSecurityKey = new SymmetricSecurityKey(key);
            var securityAlgorithm = SecurityAlgorithms.HmacSha256Signature;
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, securityAlgorithm);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.Name,user.UserName),
                        new Claim(nameof(user.Role),user.Role),
                        new Claim("UserId", user.Id.ToString()),
                    }
                    ),
                Expires = DateTime.UtcNow.AddMinutes(mainSetting.TokenExpiresInMinutes),
                SigningCredentials = signingCredentials,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            string token = tokenHandler.WriteToken(securityToken);
            return token;
        }

        public static void AttachUserToContextByToken(DatabaseContext databaseContext,HttpContext context,string token, string secretKey)
        {
            try
            {
                var key = Encoding.ASCII.GetBytes(secretKey);
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                var jwtToken = validatedToken as JwtSecurityToken;
                if (jwtToken == null)
                {
                    return;
                }
                Claim? userIdClaim = jwtToken.Claims.Where(current => current.Type.ToLower() == "UserId".ToLower()).FirstOrDefault();
                if (userIdClaim == null)
                {
                    return;
                }
                var userId = new Guid(userIdClaim.Value);
                var userFounded = databaseContext.Users.Find(userId);
                if(userFounded == null)
                {
                    return;
                }
                context.Items["User"] = userFounded;
            }
            catch
            {
                return;
            }
        }
    }
}
