using Microsoft.EntityFrameworkCore;
using project_frej.Data;
using project_frej.Models;

namespace project_frej.Services;
public class AggregationService(SensorDataContext context, ILogger<AggregationService> logger) : IAggregationService
{
    public async Task AggregateHourlyDataAsync(DateTime date, int hour)
    {
        try
        {
            var readings = await context.SensorReadings
                .Where(r => r.Timestamp.Date == date.Date && r.Timestamp.Hour == hour)
                .ToListAsync();

            if (readings.Count != 0)
            {
                var hourlyAggregate = await context.SensorReadingsHourly
                    .FirstOrDefaultAsync(a => a.Hour == new DateTime(date.Year, date.Month, date.Day, hour, 0, 0));

                if (hourlyAggregate == null)
                {
                    hourlyAggregate = new SensorReadingHourly
                    {
                        Hour = new DateTime(date.Year, date.Month, date.Day, hour, 0, 0)
                    };
                    context.SensorReadingsHourly.Add(hourlyAggregate);
                }

                hourlyAggregate.AvgTemperature = readings.Average(r => r.Temperature);
                hourlyAggregate.MinTemperature = readings.Min(r => r.Temperature);
                hourlyAggregate.MaxTemperature = readings.Max(r => r.Temperature);
                hourlyAggregate.AvgHumidity = readings.Average(r => r.Humidity);
                hourlyAggregate.MinHumidity = readings.Min(r => r.Humidity);
                hourlyAggregate.MaxHumidity = readings.Max(r => r.Humidity);
                hourlyAggregate.AvgPressure = readings.Average(r => r.Pressure);
                hourlyAggregate.MinPressure = readings.Min(r => r.Pressure);
                hourlyAggregate.MaxPressure = readings.Max(r => r.Pressure);

                await context.SaveChangesAsync();
                logger.LogInformation("Aggregated hourly data for date: {Date}, hour:{Hour}", date, hour);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while aggregating hourly data for date: {Date}, hour:{Hour}", date, hour);
        }
    }

    public async Task AggregateDailyDataAsync(DateTime date)
    {
        try
        {
            var readings = await context.SensorReadings
                .Where(r => r.Timestamp.Date == date.Date)
                .ToListAsync();

            if (readings.Count != 0)
            {
                var dailyAggregate = await context.SensorReadingsDaily
                    .FirstOrDefaultAsync(a => a.Date == date.Date);

                if (dailyAggregate == null)
                {
                    dailyAggregate = new SensorReadingDaily
                    {
                        Date = date.Date
                    };
                    context.SensorReadingsDaily.Add(dailyAggregate);
                }

                dailyAggregate.AvgTemperature = readings.Average(r => r.Temperature);
                dailyAggregate.MinTemperature = readings.Min(r => r.Temperature);
                dailyAggregate.MaxTemperature = readings.Max(r => r.Temperature);
                dailyAggregate.AvgHumidity = readings.Average(r => r.Humidity);
                dailyAggregate.MinHumidity = readings.Min(r => r.Humidity);
                dailyAggregate.MaxHumidity = readings.Max(r => r.Humidity);
                dailyAggregate.AvgPressure = readings.Average(r => r.Pressure);
                dailyAggregate.MinPressure = readings.Min(r => r.Pressure);
                dailyAggregate.MaxPressure = readings.Max(r => r.Pressure);

                await context.SaveChangesAsync();
                logger.LogInformation("Aggregated daily data for date: {Date}", date);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while aggregating daily data for date: {Date}", date);
        }
    }

    public async Task FillBlankAggregationsAsync()
    {
        logger.LogInformation("Received request to fill blank aggregations");

        try
        {
            var oldestReadingTimeStamp = await context.SensorReadings.MinAsync(r => r.Timestamp);
            var lastCompletedDay = DateTime.UtcNow.Date.AddDays(-1);
            var lastCompletedHour = DateTime.UtcNow.AddHours(-1);

            for (var date = oldestReadingTimeStamp.Date; date <= lastCompletedDay; date = date.AddDays(1))
            {
                var startHour = (date == oldestReadingTimeStamp.Date) ? oldestReadingTimeStamp.Hour : 0;
                var endHour = (date == lastCompletedDay.Date) ? lastCompletedHour.Hour : 23;

                for (var hour = startHour; hour <= endHour; hour++)
                {
                    var hourlyAggregate = await context.SensorReadingsHourly
                        .FirstOrDefaultAsync(a => a.Hour == new DateTime(date.Year, date.Month, date.Day, hour, 0, 0));

                    if (hourlyAggregate == null)
                    {
                        await AggregateHourlyDataAsync(date, hour);
                    }
                }

                if (lastCompletedDay != oldestReadingTimeStamp.Date)
                {
                    var dailyAggregate = await context.SensorReadingsDaily
                        .FirstOrDefaultAsync(a => a.Date == date.Date);

                    if (dailyAggregate == null)
                    {
                        await AggregateDailyDataAsync(date);
                    }
                }
            }

            logger.LogInformation("Filled blank aggregations");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while filling blank aggregations");
        }
    }
}
