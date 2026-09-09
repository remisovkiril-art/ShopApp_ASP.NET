namespace ShopApi.Middlewares;

public class CancellationTokenHandleMiddlewares
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CancellationTokenHandleMiddlewares> _logger;
    
    public CancellationTokenHandleMiddlewares(RequestDelegate next, ILogger<CancellationTokenHandleMiddlewares> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception _) when (_ is OperationCanceledException or TaskCanceledException)
        {
            _logger.LogError("Request canceled");
        }
    }
}

