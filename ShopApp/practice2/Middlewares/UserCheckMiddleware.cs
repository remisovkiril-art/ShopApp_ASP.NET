using ShopDomain.Models;
namespace ShopApp.practice2.Middlewares
{
    public class UserCheckMiddleware
    {
        private readonly RequestDelegate _next;
        public UserCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == "POST" &&
                context.Request.Path == "/api/user/register")
            {
                context.Request.EnableBuffering();

                User? user = await context.Request.ReadFromJsonAsync<User>();
                context.Request.Body.Position = 0;
                if (user != null &&
                    user.Login == "admin" &&
                    user.Id == 1)
                {
                    await _next(context);
                    return;
                }
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(new
                {
                    message = "No authorization"
                });
                return;
            }
            await _next(context);
        }
    }
}