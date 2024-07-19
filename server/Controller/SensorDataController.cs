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

    [HttpPost]
    public async Task<IActionResult> PostSensordata([FromBody] SensorReadingReq sensorReadingReq)
    {
        logger.LogInformation("Received sensor data: {SensorReading}", sensorReadingReq);

        var sensorReading = new SensorReading
        {
            Pressure = sensorReadingReq.Pressure,
            Temperature = sensorReadingReq.Temperature,
            Humidity = sensorReadingReq.Humidity,
            Timestamp = sensorReadingReq.Timestamp
        };

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

    [HttpPost("bulk")]
    public async Task<IActionResult> PostBulkSensordata([FromBody] IEnumerable<SensorReadingReq> sensorReadingReqs)
    {
        logger.LogInformation("Received bulk sensor data: {SensorReadingReqs}", sensorReadingReqs);

        var sensorReadings = sensorReadingReqs.Select(sr => new SensorReading
        {
            Pressure = sr.Pressure,
            Temperature = sr.Temperature,
            Humidity = sr.Humidity,
            Timestamp = sr.Timestamp
        });

        try
        {
            return Ok(await sensorDataRepository.AddBulkAsync(sensorReadings));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while adding bulk sensor data {SensorReadingReqs}", sensorReadingReqs);
            return Problem("Error while adding bulk sensor data", statusCode: 500);
        }
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
            var result = await sensorDataRepository.GetPagedAsync(pageNumber, pageSize);

            var response = new
            {
                result.TotalRecords,
                result.TotalPages,
                result.CurrentPage,
                result.PageSize,
                result.Data
            };

            return Ok(response);
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

    [HttpGet("all")]
    public async Task<IActionResult> GetAllSensorData()
    {
        try
        {
            return Ok(await sensorDataRepository.GetAllAsync());
        }
        catch (ArgumentNullException) // ToListAsync throws ArgumentNullException if no data is found
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while fetching all sensor data");
            return Problem("Error while fetching all sensor data", statusCode: 500);
        }
    }

    [HttpGet("date-range/{startDate}/{endDate}")]
    public async Task<IActionResult> GetSensorDataByDateRange(DateTime startDate, DateTime endDate)
    {
        try
        {
            return Ok(await sensorDataRepository.GetByDateRangeAsync(startDate, endDate));
        }
        catch (ArgumentNullException) // ToListAsync throws ArgumentNullException if no data is found
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while fetching sensor data by date range for {StartDate} and {EndDate}", startDate, endDate);
            return Problem("Error while fetching sensor data by date range", statusCode: 500);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutSensorData(int id, [FromBody] SensorReadingReq sensorReadingReq)
    {
        var sensorReading = new SensorReading
        {
            Pressure = sensorReadingReq.Pressure,
            Temperature = sensorReadingReq.Temperature,
            Humidity = sensorReadingReq.Humidity,
            Timestamp = sensorReadingReq.Timestamp
        };

        try
        {
            var result = await sensorDataRepository.UpdateByIdAsync(id, sensorReading);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while updating sensor data by id {Id}", id);
            return Problem("Error while updating sensor data", statusCode: 500);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSensorData(int id)
    {
        try
        {
            if (await sensorDataRepository.DeleteByIdAsync(id))
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while deleting sensor data by id {Id}", id);
            return Problem("Error while deleting sensor data", statusCode: 500);
        }
    }
}
