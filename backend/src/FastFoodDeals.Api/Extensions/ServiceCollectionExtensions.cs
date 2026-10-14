using FastFoodDeals.Core.Interfaces;
using FastFoodDeals.Infrastructure.Data;
using FastFoodDeals.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FastFoodDeals.Api.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registra la conexión a MySQL (Pomelo) y el AppDbContext.
    /// </summary>
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });

        return services;
    }

    /// <summary>
    /// Registra todos los repositorios del dominio para inyección de dependencias.
    /// </summary>
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IDealRepository, DealRepository>();
        return services;
    }

    /// <summary>
    /// Configura la política CORS para permitir solicitudes del frontend React en desarrollo.
    /// </summary>
    public static IServiceCollection AddFrontendCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins("http://localhost:5173")
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        return services;
    }
}
