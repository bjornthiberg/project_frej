using project_frej.Data;
using project_frej.Models;
using Microsoft.EntityFrameworkCore;

namespace project_frej.Services
{
    public class AggregationService
    {
        private readonly SensorDataContext _context;

        public AggregationService(SensorDataContext context)
        {
            _context = context;
        }
        public async Task AggregateDailyData(DateTime date)
        {
            var readings = await _context.SensorReadings
                .Where(r => r.Timestamp.Date == date.Date)
                .ToListAsync();

            if (readings.Count != 0)
            {
                var dailyAggregate = new SensorReadingDaily
                {
                    Date = date.Date,
                    AvgTemperature = readings.Average(r => r.Temperature),
                    MinTemperature = readings.Min(r => r.Temperature),
                    MaxTemperature = readings.Max(r => r.Temperature),
                    AvgHumidity = readings.Average(r => r.Humidity),
                    MinHumidity = readings.Min(r => r.Humidity),
                    MaxHumidity = readings.Max(r => r.Humidity),
                    AvgPressure = readings.Average(r => r.Pressure),
                    MinPressure = readings.Min(r => r.Pressure),
                    MaxPressure = readings.Max(r => r.Pressure),
                };

                _context.SensorReadingsDaily.Add(dailyAggregate);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AggregateHourlyData(DateTime date, int hour)
        {
            var readings = await _context.SensorReadings
               .Where(r => r.Timestamp.Date == date.Date && r.Timestamp.Hour == hour)
               .ToListAsync();

            if (readings.Count != 0)
            {
                var hourlyAggregate = new SensorReadingHourly
                {
                    Hour = new DateTime(date.Year, date.Month, date.Day, hour, 0, 0),
                    AvgTemperature = readings.Average(r => r.Temperature),
                    MinTemperature = readings.Min(r => r.Temperature),
                    MaxTemperature = readings.Max(r => r.Temperature),
                    AvgHumidity = readings.Average(r => r.Humidity),
                    MinHumidity = readings.Min(r => r.Humidity),
                    MaxHumidity = readings.Max(r => r.Humidity),
                    AvgPressure = readings.Average(r => r.Pressure),
                    MinPressure = readings.Min(r => r.Pressure),
                    MaxPressure = readings.Max(r => r.Pressure),
                };

                _context.SensorReadingsHourly.Add(hourlyAggregate);
                await _context.SaveChangesAsync();
            }
        }
    }
}
