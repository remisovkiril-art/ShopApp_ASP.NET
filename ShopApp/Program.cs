using ShopApp.practice2.Middlewares;
using Shop.App.Services;
using ShopApp.Interfaces;

namespace Shop.App;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddScoped<IProductService, ProductService>();

        var app = builder.Build();
        app.UseHttpsRedirection();
        app.UseMiddleware<UserCheckMiddleware>();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}