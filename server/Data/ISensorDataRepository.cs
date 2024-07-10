using project_frej.Models;

namespace project_frej.Data;

public interface ISensorDataRepository
{
    Task<IEnumerable<SensorReading>> AddAsync(SensorReading sensorReading);
    Task<IEnumerable<SensorReading>> GetByIdAsync(int id);
    Task<IEnumerable<SensorReadingHourly>> GetAggregateHourlyAsync(DateTime date, int hour);
    Task<IEnumerable<SensorReadingDaily>> GetAggregateDailyAsync(DateTime date);
    Task<(int TotalRecords, int TotalPages, int CurrentPage, int PageSize, List<SensorReading> Data)> GetPagedAsync(int pageNumber, int pageSize);
}
