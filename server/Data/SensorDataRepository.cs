using Microsoft.EntityFrameworkCore;
using project_frej.Models;
using project_frej.Services;

namespace project_frej.Data;

/// <summary>
/// Implementation of sensor data repository.
/// Provides methods for CRUD operations and data aggregation.
/// </summary>
public class SensorDataRepository(SensorDataContext context, WebSocketHandler webSocketHandler) : ISensorDataRepository
{
    /// <inheritdoc />
    public async Task<SensorReading> AddAsync(SensorReading sensorReading)
    {
        context.SensorReadings.Add(sensorReading);
        await context.SaveChangesAsync();

        // Notify WebSocket clients
        await webSocketHandler.NotifyClients(sensorReading);

        return sensorReading;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<SensorReading>> AddBulkAsync(IEnumerable<SensorReading> sensorReadings)
    {
        context.SensorReadings.AddRange(sensorReadings);
        await context.SaveChangesAsync();

        // Notify WebSocket clients for each added sensor reading
        foreach (var sensorReading in sensorReadings)
        {
            await webSocketHandler.NotifyClients(sensorReading);
        }

        return sensorReadings;
    }

    /// <inheritdoc />
    public async Task<SensorReading?> GetByIdAsync(int id)
    {
        return await context.SensorReadings.FindAsync(id);
    }

    /// <inheritdoc />
    public async Task<SensorReadingHourly?> GetAggregateHourlyAsync(DateTime date, int hour)
    {
        return await context.SensorReadingsHourly
            .Where(ha => ha.Hour.Date == date.Date && ha.Hour.Hour == hour)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<SensorReadingDaily?> GetAggregateDailyAsync(DateTime date)
    {
        return await context.SensorReadingsDaily
            .Where(da => da.Date == date.Date)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<PagedResult<SensorReading>> GetPagedAsync(int pageNumber, int pageSize)
    {
        var totalRecords = await context.SensorReadings.CountAsync();
        var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

        var sensorReadings = await context.SensorReadings
            .OrderByDescending(sr => sr.Timestamp)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .OrderBy(sr => sr.Timestamp)
            .ToListAsync();

        return new PagedResult<SensorReading>
        {
            TotalRecords = totalRecords,
            TotalPages = totalPages,
            CurrentPage = pageNumber,
            PageSize = pageSize,
            Data = sensorReadings
        };
    }

    /// <inheritdoc />
    public async Task<IEnumerable<SensorReading>> GetAllAsync()
    {
        return await context.SensorReadings
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<SensorReading>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await context.SensorReadings
            .Where(sr => sr.Timestamp >= startDate && sr.Timestamp <= endDate)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<SensorReading?> UpdateByIdAsync(int id, SensorReading sensorReading)
    {
        var existingSensorReading = await context.SensorReadings.FindAsync(id);

        if (existingSensorReading != null)
        {
            existingSensorReading.Pressure = sensorReading.Pressure;
            existingSensorReading.Temperature = sensorReading.Temperature;
            existingSensorReading.Humidity = sensorReading.Humidity;
            existingSensorReading.Timestamp = sensorReading.Timestamp;

            await context.SaveChangesAsync();
        }

        return existingSensorReading;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteByIdAsync(int id)
    {
        var sensorReading = await context.SensorReadings.FindAsync(id);

        if (sensorReading != null)
        {
            context.SensorReadings.Remove(sensorReading);
            await context.SaveChangesAsync();
            return true;
        }

        return false;
    }

}

