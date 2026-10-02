using Newtonsoft.Json;
using ShopApplication.Interfaces.Services;

namespace ShopInfrastructure.Services;

public class CurrencyService : ICurrencyService
{
    private readonly HttpClient _httpClient;

    public CurrencyService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<decimal> GetUsdRateAsync()
    {
        var response = await _httpClient.GetAsync(
            "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json");

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        dynamic data =
            JsonConvert.DeserializeObject(content)!;

        foreach (var currency in data)
        {
            if ((string)currency.cc == "USD")
            {
                return (decimal)currency.rate;
            }
        }

        throw new Exception("USD rate not found.");
    }
}
