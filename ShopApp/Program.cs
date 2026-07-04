//using ShopApi.practice2.Middlewares;
using Microsoft.EntityFrameworkCore;
using ShopApi.Interfaces;
using ShopApi.Services;
using ShopApplication.Interfaces.Repository;
using ShopApplication.Services;
using ShopInfrastructure.Data;
using ShopInfrastructure.Repositories;
using ShopApplication.Interfaces.Services;

namespace ShopApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<ShopDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });


        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });

       
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton<ShopApi.homework3.Services.IProductService, ShopApi.homework3.Services.ProductService>();

        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseCors("AllowAll");

        app.UseHttpsRedirection();
        // app.UseMiddleware<UserCheckMiddleware>();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
