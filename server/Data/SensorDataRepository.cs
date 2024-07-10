using Microsoft.EntityFrameworkCore;
using project_frej.Models;

namespace project_frej.Data;

public class SensorDataRepository(SensorDataContext context) : ISensorDataRepository
{
    private readonly SensorDataContext _context = context;

    public async Task<IEnumerable<SensorReading>> GetByIdAsync(int id)
    {
        return await _context.SensorReadings
            .Where(s => s.Id == id)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<SensorReadingHourly>> GetAggregateHourlyAsync(DateTime date, int hour)
    {
        return await _context.SensorReadingsHourly
            .Where(ha => ha.Hour.Date == date.Date && ha.Hour.Hour == hour)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<SensorReadingDaily>> GetAggregateDailyAsync(DateTime date)
    {
        return await _context.SensorReadingsDaily
            .Where(da => da.Date == date.Date)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<(int TotalRecords, int TotalPages, int CurrentPage, int PageSize, List<SensorReading> Data)> GetPagedAsync(int pageNumber, int pageSize)
    {
        var totalRecords = await _context.SensorReadings.CountAsync();
        var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

        var sensorReadings = await _context.SensorReadings
            .OrderByDescending(sr => sr.Timestamp)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        return (totalRecords, totalPages, pageNumber, pageSize, sensorReadings);
    }

    public async Task<IEnumerable<SensorReading>> AddAsync(SensorReading sensorReading)
    {
        _context.SensorReadings.Add(sensorReading);
        await _context.SaveChangesAsync();
        return await GetByIdAsync(sensorReading.Id);
    }
}

