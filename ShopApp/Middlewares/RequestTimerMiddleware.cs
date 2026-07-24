using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ShopApi.Middlewares;

public class RequestTimerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestTimerMiddleware> _logger;

    public RequestTimerMiddleware(ILogger<RequestTimerMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();

        // 1. Код ДО наступного компонента
        _logger.LogInformation("Початок запиту: {Path}", context.Request.Path);

        // 2. Передаємо керування далі (контроллер обрабатывает и отправляет JSON пользователя)
        await _next(context);

        watch.Stop();
        _logger.LogInformation("Запит завершено за {Ms} мс", watch.ElapsedMilliseconds);

        // var response = new
        // {
        //     message = "Error"
        // };

        // await context.Response.WriteAsJsonAsync(response);
        return;
    }
}


