using FastFoodDeals.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuración de base de datos MySQL (Pomelo)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Configurar CORS para permitir comunicación desde React en localhost:5173
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowFrontend");

// Carga de datos de prueba iniciales en MySQL
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(db);
}

// Endpoint de verificación de estado y conexión a MySQL
app.MapGet("/api/health/db", async (AppDbContext db) =>
{
    var canConnect = await db.Database.CanConnectAsync();
    return Results.Ok(new { status = "healthy", databaseConnected = canConnect });
});

// Endpoint inicial de consulta de cupones activos
app.MapGet("/api/deals", async (AppDbContext db) =>
{
    var deals = await db.Deals
        .Where(d => d.IsActive)
        .OrderByDescending(d => d.DiscountPercentage)
        .ToListAsync();
    return Results.Ok(deals);
});

app.Run();
