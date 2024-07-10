using Microsoft.AspNetCore.Mvc;
using project_frej.Data;
using project_frej.Models;

namespace project_frej.Controllers;

[ApiController]
[Route("api/sensorData")]
public class SensorDataController(ISensorDataRepository sensorDataRepository, ILogger<SensorDataController> logger) : ControllerBase
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
            return Ok(await sensorDataRepository.GetByIdAsync(id));
        }
        catch (ArgumentNullException) // ToListAsync throws ArgumentNullException if no data is found
        {
            return NotFound();
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
            return Ok(await sensorDataRepository.GetAggregateHourlyAsync(date, hour));
        }
        catch (ArgumentNullException) // ToListAsync throws ArgumentNullException if no data is found
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while fetching hourly sensor data for {Date} and {Hour}", date, hour);
            return Problem("Error while fetching hourly sensor data", statusCode: 500);
        }
    }

    [HttpGet("aggregate/daily/{date}")]
    public async Task<IActionResult> GetSensorDataDaily(DateTime date)
    {
        try
        {
            return Ok(await sensorDataRepository.GetAggregateDailyAsync(date));
        }
        catch (ArgumentNullException) // ToListAsync throws ArgumentNullException if no data is found
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while fetching daily sensor data for {Date}", date);
            return Problem("Error while fetching daily sensor data", statusCode: 500);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetSensorDataPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
    {
        try
        {
            return Ok(await sensorDataRepository.GetPagedAsync(pageNumber, pageSize));
        }
        catch (ArgumentNullException) // ToListAsync throws ArgumentNullException if no data is found
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while fetching paged sensor data for page {PageNumber} and page size {PageSize}", pageNumber, pageSize);
            return Problem("Error while fetching paged sensor data", statusCode: 500);
        }
    }

    [HttpPost]
    public async Task<IActionResult> PostSensordata([FromBody] SensorReading sensorReading)
    {
        logger.LogInformation("Received sensor data: {SensorReading}", sensorReading);
        try
        {
            return Ok(await sensorDataRepository.AddAsync(sensorReading));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while adding sensor data {SensorReading}", sensorReading);
            return Problem("Error while adding sensor data", statusCode: 500);
        }
    }


}
