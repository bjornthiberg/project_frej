using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using project_frej.Data;
using project_frej.Models;
using project_frej.Services;

namespace project_frej.Tests.Services
{
    public class AggregationServiceTests : IDisposable
    {
        private SqliteConnection _connection;
        private SensorDataContext _context;
        private AggregationService _service;

        public AggregationServiceTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<SensorDataContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new SensorDataContext(options);
            _context.Database.EnsureCreated();

            _service = new AggregationService(_context);
        }

        [Fact]
        public async Task AggregateDailyData_CorrectWhenReadingsExist()
        {
            var date = new DateTime(1998, 7, 16);
            _context.SensorReadings.AddRange(
                new SensorReading { Pressure = 100, Temperature = 20, Humidity = 50, Timestamp = date.AddHours(1) },
                new SensorReading { Pressure = 200, Temperature = 22, Humidity = 55, Timestamp = date.AddHours(2) }
            );

            await _context.SaveChangesAsync();

            await _service.AggregateDailyData(date);

            var aggregate = await _context.SensorReadingsDaily.FirstOrDefaultAsync(a => a.Date == date);
            Assert.NotNull(aggregate);
            Assert.Equal(150, aggregate.AvgPressure);
            Assert.Equal(200, aggregate.MaxPressure);
            Assert.Equal(100, aggregate.MinPressure);
        }

        [Fact]
        public async Task AggregateDailyData_CorrectWhenNoReadingsExist()
        {
            var date = new DateTime(1998, 7, 16);

            await _service.AggregateDailyData(date);

            var aggregate = await _context.SensorReadingsDaily.FirstOrDefaultAsync(a => a.Date == date);
            Assert.Null(aggregate);
        }

        [Fact]
        public async Task AggregateDailyData_OnlyAggregateForGivenDate()
        {
            var date = new DateTime(1998, 7, 16);
            _context.SensorReadings.AddRange(
                new SensorReading { Pressure = 100, Temperature = 20, Humidity = 50, Timestamp = date.AddHours(1) },
                new SensorReading { Pressure = 200, Temperature = 22, Humidity = 55, Timestamp = date.AddDays(1) }
            );

            await _context.SaveChangesAsync();

            await _service.AggregateDailyData(date);

            var aggregate = await _context.SensorReadingsDaily.FirstOrDefaultAsync(a => a.Date == date);
            Assert.NotNull(aggregate);
            Assert.Equal(100, aggregate.AvgPressure);
            Assert.Equal(100, aggregate.MaxPressure);
            Assert.Equal(100, aggregate.MinPressure);
        }

        [Fact]
        public async Task AggregateHourlyData_CorrectWhenReadingsExist()
        {
            var date = new DateTime(1998, 7, 16);
            _context.SensorReadings.AddRange(
                new SensorReading { Pressure = 100, Temperature = 20, Humidity = 50, Timestamp = date.AddHours(1) },
                new SensorReading { Pressure = 200, Temperature = 22, Humidity = 55, Timestamp = date.AddHours(1) }
            );

            await _context.SaveChangesAsync();

            await _service.AggregateHourlyData(date, 1);

            var aggregate = await _context.SensorReadingsHourly.FirstOrDefaultAsync(a => a.Hour == date.AddHours(1));
            Assert.NotNull(aggregate);
            Assert.Equal(150, aggregate.AvgPressure);
            Assert.Equal(200, aggregate.MaxPressure);
            Assert.Equal(100, aggregate.MinPressure);
        }

        [Fact]
        public async Task AggregateHourlyData_CorrectWhenNoReadingsExist()
        {
            var date = new DateTime(1998, 7, 16);

            await _service.AggregateHourlyData(date, 1);

            var aggregate = await _context.SensorReadingsHourly.FirstOrDefaultAsync(a => a.Hour == date.AddHours(1));
            Assert.Null(aggregate);
        }

        [Fact]
        public async Task AggregateHourlyData_OnlyAggregateForGivenDateAndHour()
        {
            var date = new DateTime(1998, 7, 16);
            _context.SensorReadings.AddRange(
                new SensorReading { Pressure = 100, Temperature = 20, Humidity = 50, Timestamp = date.AddHours(1) },
                new SensorReading { Pressure = 200, Temperature = 22, Humidity = 55, Timestamp = date.AddHours(2) }
            );

            await _context.SaveChangesAsync();

            await _service.AggregateHourlyData(date, 1);

            var aggregate = await _context.SensorReadingsHourly.FirstOrDefaultAsync(a => a.Hour == date.AddHours(1));
            Assert.NotNull(aggregate);
            Assert.Equal(100, aggregate.AvgPressure);
            Assert.Equal(100, aggregate.MaxPressure);
            Assert.Equal(100, aggregate.MinPressure);

        }

        public void Dispose()
        {
            _context.Dispose();
            _connection.Dispose();
        }
    }
}
