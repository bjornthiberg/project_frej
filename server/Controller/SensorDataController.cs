using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_frej.Data;
using project_frej.Models;

namespace project_frej.Controllers;

[ApiController]
[Route("api/sensorData")]
public class SensorDataController(SensorDataContext context, ILogger<SensorDataController> logger) : ControllerBase
{
    [HttpGet("/")]
    public IActionResult GetRoot()
    {
        return Ok("API for Project Frej");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSensorData(int id)
    {
        var sensorReading = await context.SensorReadings.FindAsync(id);
        if (sensorReading == null)
        {
            logger.LogWarning("SensorReading with ID {Id} not found", id);
            return NotFound();
        }

        logger.LogInformation("SensorReading with ID {Id} provided", id);
        return Ok(sensorReading);
    }

    [HttpGet("aggregate/hourly/{date}/{hour}")]
    public async Task<IActionResult> GetSensorDataHourly(DateTime date, int hour)
    {
        var hourlyAggregates = await context.SensorReadingsHourly
            .Where(ha => ha.Hour.Date == date.Date && ha.Hour.Hour == hour)
            .ToListAsync();

        return hourlyAggregates.Count != 0 ? Ok(hourlyAggregates) : NotFound();
    }

    [HttpGet("aggregated/daily/{date}")]
    public async Task<IActionResult> GetSensorDataDaily(DateTime date)
    {
        var dailyAggregates = await context.SensorReadingsDaily
            .Where(da => da.Date == date.Date)
            .ToListAsync();

        return dailyAggregates.Count != 0 ? Ok(dailyAggregates) : NotFound();
    }


    [HttpGet("latest")]
    public async Task<IActionResult> GetLatestSensorData([FromQuery] int pageSize = 100)
    {
        var totalRecords = await context.SensorReadings.CountAsync();

        var sensorReadings = await context.SensorReadings
            .OrderByDescending(sr => sr.Timestamp)
            .Take(pageSize)
            .ToListAsync();

        var response = new
        {
            TotalRecords = totalRecords,
            Data = sensorReadings
        };

        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetSensorData([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
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

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> PostSensordata([FromBody] SensorReading sensorReading)
    {
        logger.LogInformation("Received sensor data: {SensorReading}", sensorReading);
        try
        {
            context.SensorReadings.Add(sensorReading);
            await context.SaveChangesAsync();

            return Created($"/api/sensorData/{sensorReading.Id}", sensorReading);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while processing sensor data");
            return Problem("Error while processing sensor data", statusCode: 500);
        }
    }


}
