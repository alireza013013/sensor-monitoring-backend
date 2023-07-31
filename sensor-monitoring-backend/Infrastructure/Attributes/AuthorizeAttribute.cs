using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using sensor_monitoring_backend.Domain.Entities;

namespace sensor_monitoring_backend.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : System.Attribute, IAuthorizationFilter
    {
        public AuthorizeAttribute()
        {
        }
        public AuthorizeAttribute(string userRole)
        {
            _userRole = userRole;   
        }

        protected string _userRole { get; }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            User? user = context.HttpContext.Items["User"] as User;
            if (user == null)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
            if(_userRole != null)
            {
                if (user?.Role.ToLower() != _userRole.ToLower())
                {
                    context.Result = new JsonResult(new { message = "Access Denied" })
                    {
                        StatusCode = StatusCodes.Status403Forbidden
                    };
                }
            }
        }
    }
}
