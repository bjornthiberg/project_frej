using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using project_frej.Data;
using project_frej.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace project_frej.Tests.TestUtils
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                var settings = new List<KeyValuePair<string, string?>>
                {
                    new("ApiKey", "testing"),
                    new("ConnectionStrings:DefaultConnection", "DataSource=:memory:")
                };
                config.AddInMemoryCollection(settings);
            });

            builder.ConfigureServices(services =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                services.RemoveAll(typeof(DbContextOptions<SensorDataContext>));

                services.AddDbContext<SensorDataContext>(options =>
                {
                    options.UseSqlite(connection);
                });

                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<SensorDataContext>();

                db.Database.EnsureCreated();

                db.SensorReadings.Add(new SensorReading
                {
                    Pressure = 100,
                    Temperature = 20,
                    Humidity = 50,
                    Lux = 300,
                    Uvs = 1,
                    Gas = 10,
                    Timestamp = new DateTime(1998, 7, 16)
                });

                db.SaveChanges();
            });

            builder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();

                logging.AddConsole()
                    .SetMinimumLevel(LogLevel.Warning);
            });
        }
    }
}
