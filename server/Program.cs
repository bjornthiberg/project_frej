using project_frej.Models;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "API for Project Frej");

app.MapPost("/api/sensorData", (SensorReading sensorReading, ILogger<Program> logger) =>
{
    logger.LogInformation("Received sensor data: {sensorReading}", sensorReading);

    // TODO: Add logic to save the sensor data to the database

    try
    {
        // Simulate data processing
        return Results.Ok(sensorReading);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error while processing sensor data");
        return Results.StatusCode(500);
    }
});

app.Run();

