namespace ShopApplication.Interfaces.Services;

public interface ICurrencyService
{
    Task<decimal> GetUsdRateAsync();
}