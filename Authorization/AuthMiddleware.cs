using System.Net.Http.Headers;
using System.Text;

namespace API.Response.Filter.Authorization
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                

                var authHeader = AuthenticationHeaderValue.Parse(context.Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
                var username = credentials[0];
                var password = credentials[1];

                // authenticate credentials with user service and attach user to http context
                // context.Items["User"] = await userService.Authenticate(username, password);
                context.Items["User"] = "True";
            }
            catch
            {
                // do nothing if invalid auth header
                // user is not attached to context so request won't have access to secure routes
            }

            await _next(context);
        }
    }
}
