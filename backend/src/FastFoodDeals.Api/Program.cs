using FastFoodDeals.Api.Extensions;
using FastFoodDeals.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddScrapers();
builder.Services.AddFrontendCors();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Seed de datos iniciales de prueba (solo si la BD estuviera vacía)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(db);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowFrontend");
app.MapControllers();

app.Run();
