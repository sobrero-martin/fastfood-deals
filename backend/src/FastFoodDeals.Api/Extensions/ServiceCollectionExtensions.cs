using FastFoodDeals.Core.Interfaces;
using FastFoodDeals.Infrastructure.Data;
using FastFoodDeals.Infrastructure.Repositories;
using FastFoodDeals.Infrastructure.Scrapers;
using Microsoft.EntityFrameworkCore;

namespace FastFoodDeals.Api.Extensions;

public static class ServiceCollectionExtensions
{
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

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IDealRepository, DealRepository>();
        return services;
    }

    public static IServiceCollection AddScrapers(this IServiceCollection services)
    {
        services.AddHttpClient<McDonaldsScraper>();
        services.AddScoped<McDonaldsScraper>();
        return services;
    }

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
