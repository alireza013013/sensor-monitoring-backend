using Microsoft.Extensions.Options;
using sensor_monitoring_backend.Data;
using sensor_monitoring_backend.Infrastructure.ApplicationSettings;

namespace sensor_monitoring_backend.Infrastructure.Middlewares
{
    public class JwtMiddleware : object
    {
        public JwtMiddleware(RequestDelegate next,IOptions<Main> options)
        {
            _next = next;
            _mainSettings = options.Value;
        }

        protected Main _mainSettings { get; }

        protected RequestDelegate _next { get; }



        public async Task Invoke(HttpContext context, DatabaseContext databaseContext)
        {
            var requestHeaders = context.Request.Headers["Authorization"];

            string? token = requestHeaders.FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrWhiteSpace(token) == false)
            {
                JwtUtility.AttachUserToContextByToken(databaseContext, context,token,_mainSettings.SecretKey);
            }

            await _next(context);
        }
    }
}
