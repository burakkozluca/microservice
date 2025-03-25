using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Services.Identity.Data;
using Services.Identity.Data.Entities;
using Services.Identity.Services.Auth;
using Services.Identity.Services.Users;

namespace Services.Identity.Extensions;

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
        
        // Identity Configuration
        services.AddIdentity<User, IdentityRole>(options => 
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 8;
        }).AddEntityFrameworkStores<AppDbContext>()
          .AddDefaultTokenProviders();

        
        // Application Services
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IUserService, UserService>();
        
        // AutoMapper Configuration
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // JWT Configuration
        services.Configure<CustomTokenOption>(configuration.GetSection("TokenOption"));

        // Authentication Configuration
        services.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(opt =>
        {
            var tokenOption = configuration.GetSection("TokenOption").Get<CustomTokenOption>();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOption.SecurityKey));
            
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = tokenOption.Audience,
                ValidIssuer = tokenOption.Issuer,
                IssuerSigningKey = securityKey,
                ClockSkew = TimeSpan.Zero
            };
        });

        return services;
    }
}