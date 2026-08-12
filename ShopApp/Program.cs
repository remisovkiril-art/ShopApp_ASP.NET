using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShopApi.Interfaces;
using ShopApi.Middlewares;
using ShopApi.Services;
using ShopApplication.Interfaces.Helpers;
using ShopApplication.Interfaces.Repository;
using ShopApplication.Interfaces.Services;
using ShopApplication.Mapping;
using ShopApplication.Services;
using ShopInfrastructure.Configuration;
using ShopInfrastructure.Data;
using ShopInfrastructure.Helpers;
using ShopInfrastructure.Repositories;
using ShopInfrastructure.Services;
using System;
using System.IO;
using System.Text;

namespace ShopApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;

        builder.Services.AddDbContext<ShopDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SqlServerConnection"));
        });

        // ================= JWT Settings =================
        var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>()
            ?? throw new Exception("JWT settings not configured.");
        builder.Services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
        builder.Services.AddScoped<IJWTService, JWTService>();

        // ================= Authentication =================
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.Key)
                ),

                ClockSkew = TimeSpan.Zero
            };
        });

        builder.Services.AddAuthorization();

        // ================= AutoMapper =================
        builder.Services.AddAutoMapper(
            _ => { },
            typeof(CategoryProfile).Assembly
        );

        // ================= CORS =================
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });
        //builder.Services.AddCors(options =>
        //{
        //    options.AddPolicy("ProductionPolicy", policy =>
        //    {
        //        policy.WithOrigins("https://example.com", "https://www.example.com")
        //              .WithMethods("GET", "POST", "PUT", "DELETE")
        //              .WithHeaders("Content-Type", "Authorization");
        //    });
        //});
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Description = "Enter JWT token"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });


        //--------------SERVICES-------------------
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IAdminService, AdminService>();
        builder.Services.AddScoped<IImageService, ImageService>();
        builder.Services.AddSingleton<IHashHelper, HashHelper>();
        // ================= CACHE =================
        builder.Services.AddMemoryCache();
        builder.Services.AddScoped<ICachingService, MemoryCachingService>();
        //--------------REPOSITORIES
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<IAuthRepository, AuthRepository>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        var app = builder.Build();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseCors("AllowAll");
        //app.UseCors("ProductionPolicy");
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<RequestTimerMiddleware>();
        app.UseStaticFiles();
        app.MapControllers();

        app.Run();
    }
}



