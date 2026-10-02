using Polly;

namespace ShopInfrastructure.Helpers;

public static class PollyHelper
{
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .OrResult(response => !response.IsSuccessStatusCode)
            .WaitAndRetryAsync(
                3,
                retryAttempt =>
                    TimeSpan.FromSeconds(
                        Math.Pow(2, retryAttempt)),
                (result, timeSpan, retryCount, context) =>
                {
                    Console.WriteLine(
                        $"Retry {retryCount}");
                });
    }

    public static IAsyncPolicy<HttpResponseMessage>
        GetCircuitBreakerPolicy()
    {
        return Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .OrResult(response => !response.IsSuccessStatusCode)
            .CircuitBreakerAsync(
                2,
                TimeSpan.FromSeconds(30));
    }
}