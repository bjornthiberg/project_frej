using project_frej.Models;

namespace project_frej.Data;

public interface ISensorDataRepository
{
    Task<SensorReading> AddAsync(SensorReading sensorReading);
    Task<IEnumerable<SensorReading>> AddSensorDataBulkAsync(IEnumerable<SensorReading> sensorReadings);
    Task<SensorReading?> GetByIdAsync(int id);
    Task<SensorReadingHourly?> GetAggregateHourlyAsync(DateTime date, int hour);
    Task<SensorReadingDaily?> GetAggregateDailyAsync(DateTime date);
    Task<PagedResult<SensorReading>> GetPagedAsync(int pageNumber, int pageSize);
    Task<IEnumerable<SensorReading>> GetAllSensorDataAsync();
    Task<IEnumerable<SensorReading>> GetSensorDataByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<SensorReading?> UpdateSensorDataByIdAsync(int id, SensorReading sensorReading);
    Task<bool> DeleteSensorDataByIdAsync(int id);
}
