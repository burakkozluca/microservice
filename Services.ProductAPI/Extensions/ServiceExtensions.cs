using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Services.ProductAPI.Data;
using Services.ProductAPI.Helpers;
using Services.ProductAPI.Services;

namespace Services.ProductAPI.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Database Configuration
        services.AddDbContext<AppDbContext>(options =>
        {
            var connectionString = 
                configuration.GetSection(ConnectionStringOption.Key).Get<ConnectionStringOption>();

            options.UseNpgsql(connectionString!.PostgreSql, npgsqlOptionsAction =>
            {
                npgsqlOptionsAction.MigrationsAssembly(typeof(DataAssembly).Assembly.FullName);
            });
        });
        

        
        // Application Services
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IFileService, FileService>();
        
        // AutoMapper Configuration
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        

        return services;
    }
}