using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Services.ShoppingCart.API.Data;
using Services.ShoppingCart.API.Services;

namespace Services.ShoppingCart.API.Extensions;

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
        services.AddScoped<ICartService, CartService>();
        
        // AutoMapper Configuration
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        return services;
    }
}