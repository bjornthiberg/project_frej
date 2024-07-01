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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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

app.MapGet("/api/sensorData/hourly", async (DateTime date, int hour, ILogger<Program> logger, SensorDataContext context) =>
{
    var hourlyAggregates = await context.SensorReadingsHourly
        .Where(ha => ha.Hour.Date == date.Date && ha.Hour.Hour == hour)
        .ToListAsync();

    return hourlyAggregates.Count != 0 ? Results.Ok(hourlyAggregates) : Results.NotFound();
});

app.MapGet("/api/sensorData/daily", async (DateTime date, ILogger<Program> logger, SensorDataContext context) =>
{
    var dailyAggregates = await context.SensorReadingsDaily
        .Where(da => da.Date == date.Date)
        .ToListAsync();

    return dailyAggregates.Count != 0 ? Results.Ok(dailyAggregates) : Results.NotFound();
});

app.MapGet("/api/sensorData", async (SensorDataContext context, int pageNumber = 1, int pageSize = 100) =>
{
    var totalRecords = await context.SensorReadings.CountAsync();
    var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

    var sensorReadings = await context.SensorReadings
        .OrderBy(sr => sr.Timestamp)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    var response = new
    {
        TotalRecords = totalRecords,
        TotalPages = totalPages,
        CurrentPage = pageNumber,
        PageSize = pageSize,
        Data = sensorReadings
    };

    return Results.Ok(response);
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

