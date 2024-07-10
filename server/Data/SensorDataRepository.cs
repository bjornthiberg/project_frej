using Microsoft.EntityFrameworkCore;
using project_frej.Models;

namespace project_frej.Data;

public class SensorDataRepository(SensorDataContext context) : ISensorDataRepository
{
    private readonly SensorDataContext _context = context;

    public async Task<IEnumerable<SensorReading>> GetSensorDataByIdAsync(int id)
    {
        return await _context.SensorReadings
            .Where(s => s.Id == id)
            .ToListAsync();
    }

    public async Task<IEnumerable<SensorReadingHourly>> GetSensorDataAggregateHourlyAsync(DateTime date, int hour)
    {
        return await _context.SensorReadingsHourly
            .Where(ha => ha.Hour.Date == date.Date && ha.Hour.Hour == hour)
            .ToListAsync();
    }

    public async Task<IEnumerable<SensorReadingDaily>> GetSensorDataAggregateDailyAsync(DateTime date)
    {
        return await _context.SensorReadingsDaily
            .Where(da => da.Date == date.Date)
            .ToListAsync();
    }

    public async Task<(int TotalRecords, int TotalPages, int CurrentPage, int PageSize, List<SensorReading> Data)> GetSensorDataPagedAsync(int pageNumber, int pageSize)
    {
        var totalRecords = await _context.SensorReadings.CountAsync();
        var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

        var sensorReadings = await _context.SensorReadings
            .OrderByDescending(sr => sr.Timestamp)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (totalRecords, totalPages, pageNumber, pageSize, sensorReadings);
    }

    public async Task<IEnumerable<SensorReading>> AddSensorDataAsync(SensorReading sensorReading)
    {
        _context.SensorReadings.Add(sensorReading);
        await _context.SaveChangesAsync();
        return await GetSensorDataByIdAsync(sensorReading.Id);
    }
}

