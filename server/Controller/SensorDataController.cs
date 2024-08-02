using Microsoft.AspNetCore.Mvc;
using project_frej.Data;
using project_frej.Models;
using project_frej.Services;

namespace project_frej.Controllers;

/// <summary>
/// API controller for CRUD operations of sensor data.
/// </summary>
[ApiController]
[Route("api/sensorData")]
public class SensorDataController(ISensorDataRepository sensorDataRepository, ILogger<SensorDataController> logger, WebSocketHandler webSocketHandler) : ControllerBase
{
    /// <summary>
    /// Returns a simple "API for Project Frej" response for the root endpoint.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> containing the string "API for Project Frej".</returns>
    [HttpGet("/")]
    public IActionResult GetRoot()
    {
        return Ok("API for Project Frej");
    }

    /// <summary>
    /// Adds a new sensor reading to the repository.
    /// </summary>
    /// <param name="sensorReadingReq">The sensor reading to be added.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing:
    /// <list type="bullet">
    /// <item><description>The newly added sensor reading if successful.</description></item>
    /// <item><description>A <see cref="Problem"/> result with status code 500 if an error occurs.</description></item>
    /// </list>
    /// </returns>
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

    /// <summary>
    /// Adds a collection of sensor readings to the repository.
    /// </summary>
    /// <param name="sensorReadingReqs">The collection of sensor readings to be added.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing:
    /// <list type="bullet">
    /// <item><description>The newly added sensor readings if successful.</description></item>
    /// <item><description>A <see cref="Problem"/> result with status code 500 if an error occurs.</description></item>
    /// </list>
    /// </returns>
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

    /// <summary>
    /// Retrieves the sensor data for the specified ID.
    /// </summary>
    /// <param name="id">The ID of the sensor data to retrieve.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing:
    /// <list type="bullet">
    /// <item><description>The sensor data with the specified ID, if found.</description></item>
    /// <item><description>A <see cref="NotFoundResult"/> if no sensor data is found with the specified ID.</description></item>
    /// <item><description>A <see cref="Problem"/> result with status code 500 if an error occurs.</description></item>
    /// </list>
    /// </returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSensorData(int id)
    {
        try
        {
            var sensorData = await sensorDataRepository.GetByIdAsync(id);
            if (sensorData == null)
            {
                return NotFound();
            }
            return Ok(sensorData);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while fetching sensor data by ID {Id}", id);
            return Problem(detail: ex.Message, statusCode: 500);
        }
    }

    /// <summary>
    /// Retrieves the aggregated sensor data for a specific hour of a given date.
    /// </summary>
    /// <param name="date">The date for which to retrieve the aggregated sensor data in the format YYYY-MM-DD.</param>
    /// <param name="hour">The hour of the day (0-23) for which to retrieve the aggregated sensor data.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing:
    /// <list type="bullet">
    /// <item><description>The aggregated sensor data for the specified date and hour, if found.</description></item>
    /// <item><description>A <see cref="NotFoundResult"/> if no data is found for the specified date and hour.</description></item>
    /// <item><description>A <see cref="Problem"/> result with status code 500 if an error occurs.</description></item>
    /// </list>
    /// </returns>
    [HttpGet("aggregate/hourly/{date}/{hour}")]
    public async Task<IActionResult> GetSensorDataHourly(DateTime date, int hour)
    {
        try
        {
            return Ok(await sensorDataRepository.GetAggregateHourlyAsync(date, hour));
        }
        catch (ArgumentNullException) // GetAggregateHourlyAsync throws ArgumentNullException if no data is found
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while fetching hourly sensor data for {Date} and {Hour}", date, hour);
            return Problem("Error while fetching hourly sensor data", statusCode: 500);
        }
    }

    /// <summary>
    /// Retrieves the aggregated sensor data for a specific day.
    /// </summary>
    /// <param name="date">The date for which to retrieve the aggregated sensor data in the format YYYY-MM-DD.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing:
    /// <list type="bullet">
    /// <item><description>The aggregated sensor data for the specified date, if found.</description></item>
    /// <item><description>A <see cref="NotFoundResult"/> if no data is found for the specified date.</description></item>
    /// <item><description>A <see cref="Problem"/> result with status code 500 if an error occurs.</description></item>
    /// </list>
    /// </returns>
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
            return StatusCode(500, "Error while fetching daily sensor data");
        }
    }

    /// <summary>
    /// Retrieves sensor data in a paginated format.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve. Defaults to 1.</param>
    /// <param name="pageSize">The number of records per page. Defaults to 100.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing:
    /// <list type="bullet">
    /// <item><description>A paginated list of sensor data, if found.</description></item>
    /// <item><description>A <see cref="NotFoundResult"/> if no data is found for the specified page.</description></item>
    /// <item><description>A <see cref="Problem"/> result with status code 500 if an error occurs.</description></item>
    /// </list>
    /// </returns>
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
            logger.LogError(ex, "Error while fetching paged sensor data for page {pageNumber} and page size {pageSize}", pageNumber, pageSize);
            return Problem("Error while fetching paged sensor data", statusCode: 500);
        }
    }

    /// <summary>
    /// Handles WebSocket connections for streaming sensor data.
    /// </summary>
    /// <returns>
    /// An asynchronous task that completes when the WebSocket connection is closed.
    /// </returns>
    /// <remarks>
    /// This endpoint establishes a WebSocket connection to stream sensor data in real-time.
    /// If an error occurs during the WebSocket connection handling, a 500 Internal Server Error response is sent.
    /// </remarks>
    [HttpGet("websocket")]
    public async Task GetSensorDataWebsocket()
    {
        try
        {
            await webSocketHandler.HandleWebSocketAsync(HttpContext);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while handling WebSocket connection");
            HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await HttpContext.Response.WriteAsync("Error while handling WebSocket connection");
        }
    }

    /// <summary>
    /// Retrieves all sensor data.
    /// </summary>
    /// <returns>
    /// An <see cref="IActionResult"/> containing:
    /// <list type="bullet">
    /// <item><description>All sensor data if found.</description></item>
    /// <item><description>A <see cref="NotFoundResult"/> if no data is found.</description></item>
    /// <item><description>A <see cref="Problem"/> result with status code 500 if an error occurs.</description></item>
    /// </list>
    /// </returns>
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


    /// <summary>
    /// Retrieves sensor data within the specified date range.
    /// </summary>
    /// <param name="startDate">The start date of the date range.</param>
    /// <param name="endDate">The end date of the date range.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing:
    /// <list type="bullet">
    /// <item><description>The sensor data within the specified date range if found.</description></item>
    /// <item><description>A <see cref="NotFoundResult"/> if no data is found.</description></item>
    /// <item><description>A <see cref="Problem"/> result with status code 500 if an error occurs.</description></item>
    /// </list>
    /// </returns>
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

    /// <summary>
    /// Updates the sensor data for the specified sensor ID.
    /// </summary>
    /// <param name="id">The ID of the sensor data to update.</param>
    /// <param name="sensorReadingReq">The updated sensor reading data.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing:
    /// <list type="bullet">
    /// <item><description>The updated sensor data if the update was successful.</description></item>
    /// <item><description>A <see cref="NotFoundResult"/> if the sensor data with the specified ID was not found.</description></item>
    /// <item><description>A <see cref="Problem"/> result with status code 500 if an error occurs.</description></item>
    /// </list>
    /// </returns>
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
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while updating sensor data by id {Id}", id);
            return Problem("Error while updating sensor data", statusCode: 500);
        }
    }

    /// <summary>
    /// Deletes the sensor data with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the sensor data to delete.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing:
    /// <list type="bullet">
    /// <item><description>An <see cref="OkResult"/> if the sensor data was successfully deleted.</description></item>
    /// <item><description>A <see cref="NotFoundResult"/> if the sensor data with the specified ID was not found.</description></item>
    /// <item><description>A <see cref="Problem"/> result with status code 500 if an error occurs.</description></item>
    /// </list>
    /// </returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSensorData(int id)
    {
        try
        {
            if (await sensorDataRepository.DeleteByIdAsync(id))
            {
                return Ok();
            }

            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while deleting sensor data by id {Id}", id);
            return Problem("Error while deleting sensor data", statusCode: 500);
        }
    }
}
