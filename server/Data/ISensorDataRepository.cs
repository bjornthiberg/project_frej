using project_frej.Models;

namespace project_frej.Data;

public interface ISensorDataRepository
{
    Task<IEnumerable<SensorReading>> AddSensorDataAsync(SensorReading sensorReading);
    Task<IEnumerable<SensorReading>> GetSensorDataByIdAsync(int id);
    Task<IEnumerable<SensorReadingHourly>> GetSensorDataAggregateHourlyAsync(DateTime date, int hour);
    Task<IEnumerable<SensorReadingDaily>> GetSensorDataAggregateDailyAsync(DateTime date);
    Task<(int TotalRecords, int TotalPages, int CurrentPage, int PageSize, List<SensorReading> Data)> GetSensorDataPagedAsync(int pageNumber, int pageSize);
}
