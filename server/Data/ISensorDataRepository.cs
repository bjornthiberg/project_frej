using project_frej.Models;

namespace project_frej.Data;

/// <summary>
/// Interface for sensor data repository.
/// Provides methods for CRUD operations and data aggregation.
/// </summary>
public interface ISensorDataRepository
{
    /// <summary>
    /// Adds a new sensor reading asynchronously.
    /// </summary>
    /// <param name="sensorReading">The sensor reading to add.</param>
    /// <returns>The added sensor reading.</returns>
    Task<SensorReading> AddAsync(SensorReading sensorReading);

    /// <summary>
    /// Adds multiple sensor readings asynchronously.
    /// </summary>
    /// <param name="sensorReadings">The sensor readings to add.</param>
    /// <returns>The added sensor readings.</returns>
    Task<IEnumerable<SensorReading>> AddBulkAsync(IEnumerable<SensorReading> sensorReadings);

    /// <summary>
    /// Gets a sensor reading by ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the sensor reading.</param>
    /// <returns>The sensor reading with the specified ID, or null if not found.</returns>
    Task<SensorReading?> GetByIdAsync(int id);

    /// <summary>
    /// Gets hourly aggregated sensor data for a specific date and hour asynchronously.
    /// </summary>
    /// <param name="date">The date for the aggregation.</param>
    /// <param name="hour">The hour for the aggregation.</param>
    /// <returns>The hourly aggregated sensor data, or null if not found.</returns>
    Task<SensorReadingHourly?> GetAggregateHourlyAsync(DateTime date, int hour);

    /// <summary>
    /// Gets daily aggregated sensor data for a specific date asynchronously.
    /// </summary>
    /// <param name="date">The date for the aggregation.</param>
    /// <returns>The daily aggregated sensor data, or null if not found.</returns>
    Task<SensorReadingDaily?> GetAggregateDailyAsync(DateTime date);

    /// <summary>
    /// Gets paged sensor data asynchronously.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of records per page.</param>
    /// <returns>The paged result containing the sensor readings.</returns>
    Task<PagedResult<SensorReading>> GetPagedAsync(int pageNumber, int pageSize);

    /// <summary>
    /// Gets all sensor readings asynchronously.
    /// </summary>
    /// <returns>A collection of all sensor readings.</returns>
    Task<IEnumerable<SensorReading>> GetAllAsync();

    /// <summary>
    /// Gets sensor readings within a specific date range asynchronously.
    /// </summary>
    /// <param name="startDate">The start date of the range.</param>
    /// <param name="endDate">The end date of the range.</param>
    /// <returns>A collection of sensor readings within the specified date range.</returns>
    Task<IEnumerable<SensorReading>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Updates a sensor reading by ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the sensor reading to update.</param>
    /// <param name="sensorReading">The updated sensor reading.</param>
    /// <returns>The updated sensor reading, or null if not found.</returns>
    Task<SensorReading?> UpdateByIdAsync(int id, SensorReading sensorReading);

    /// <summary>
    /// Deletes a sensor reading by ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the sensor reading to delete.</param>
    /// <returns>True if the sensor reading was deleted, false otherwise.</returns>
    Task<bool> DeleteByIdAsync(int id);
}
