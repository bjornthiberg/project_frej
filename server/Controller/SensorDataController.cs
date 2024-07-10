using Microsoft.AspNetCore.Mvc;
using project_frej.Data;
using project_frej.Models;

namespace project_frej.Controllers;

[ApiController]
[Route("api/sensorData")]
public class SensorDataController(ISensorDataRepository repository, ILogger<SensorDataController> logger) : ControllerBase
{
    [HttpGet("/")]
    public IActionResult GetRoot()
    {
        return Ok("API for Project Frej");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSensorData(int id)
    {
        try
        {
            return Ok(await repository.GetSensorDataByIdAsync(id));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while fetching sensor data by id");
            return Problem("Error while fetching sensor data", statusCode: 500);
        }
    }

    [HttpGet("aggregate/hourly/{date}/{hour}")]
    public async Task<IActionResult> GetSensorDataHourly(DateTime date, int hour)
    {
        try
        {
            return Ok(await repository.GetSensorDataAggregateHourlyAsync(date, hour));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while fetching hourly sensor data");
            return Problem("Error while fetching hourly sensor data", statusCode: 500);
        }
    }

    [HttpGet("aggregate/daily/{date}")]
    public async Task<IActionResult> GetSensorDataDaily(DateTime date)
    {
        try
        {
            return Ok(await repository.GetSensorDataAggregateDailyAsync(date));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while fetching daily sensor data");
            return Problem("Error while fetching daily sensor data", statusCode: 500);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetSensorDataPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
    {
        try
        {
            return Ok(await repository.GetSensorDataPagedAsync(pageNumber, pageSize));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while fetching paged sensor data");
            return Problem("Error while fetching paged sensor data", statusCode: 500);
        }
    }

    [HttpPost]
    public async Task<IActionResult> PostSensordata([FromBody] SensorReading sensorReading)
    {
        logger.LogInformation("Received sensor data: {SensorReading}", sensorReading);
        try
        {
            return Ok(await repository.AddSensorDataAsync(sensorReading));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while adding sensor data");
            return Problem("Error while adding sensor data", statusCode: 500);
        }
    }


}
