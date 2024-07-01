using project_frej.Models;
using project_frej.Data;
using project_frej.Middleware;
using project_frej.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SensorDataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<AggregationService>();
builder.Services.AddHostedService<AggregationHostedService>();

var app = builder.Build();

app.UseMiddleware<ApiKeyPostMiddleware>();

app.MapGet("/", () => "API for Project Frej");

app.MapGet("/api/sensorData/{id}", async (int id, SensorDataContext db, ILogger<Program> logger) =>
{
    var sensorReading = await db.SensorReadings.FindAsync(id);
    if (sensorReading == null)
    {
        logger.LogWarning("SensorReading with ID {Id} not found", id);
        return Results.NotFound();
    }

    logger.LogInformation("SensorReading with ID {Id} provided", id);
    return Results.Ok(sensorReading);
});

app.MapPost("/api/sensorData", async (SensorReading sensorReading, ILogger<Program> logger, SensorDataContext db) =>
{
    logger.LogInformation("Received sensor data: {sensorReading}", sensorReading);

    try
    {
        db.SensorReadings.Add(sensorReading);
        await db.SaveChangesAsync();

        return Results.Created($"/api/sensorData/{sensorReading.Id}", sensorReading);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error while processing sensor data");
        return Results.StatusCode(500);
    }
});

app.Run();

