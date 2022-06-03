using DailyLogProject.Helpers;
using DailyLogProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace DailyLogProject.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var user = Extensions.ValidateToken(token, _appSettings.Secret);
            if (user != null)
            {
                // attach user to context on successful jwt validation
                context.Items["User"] = userService.IsUserExist(user);
            }

            await _next(context);
        }
    }
}
