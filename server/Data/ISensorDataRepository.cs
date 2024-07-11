using project_frej.Models;

namespace project_frej.Data;

public interface ISensorDataRepository
{
    Task<SensorReading> AddAsync(SensorReading sensorReading);
    Task<IEnumerable<SensorReading>> AddBulkAsync(IEnumerable<SensorReading> sensorReadings);
    Task<SensorReading?> GetByIdAsync(int id);
    Task<SensorReadingHourly?> GetAggregateHourlyAsync(DateTime date, int hour);
    Task<SensorReadingDaily?> GetAggregateDailyAsync(DateTime date);
    Task<PagedResult<SensorReading>> GetPagedAsync(int pageNumber, int pageSize);
    Task<IEnumerable<SensorReading>> GetAllAsync();
    Task<IEnumerable<SensorReading>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<SensorReading?> UpdateByIdAsync(int id, SensorReading sensorReading);
    Task<bool> DeleteByIdAsync(int id);
}
