using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;
using project_frej.Data;
using Microsoft.Data.Sqlite;
using project_frej.Models;

namespace project_frej.Tests.TestUtils
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Create a new SQLite connection that will be used by the context.
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                // Remove the app's SensorDataContext registration.
                services.RemoveAll(typeof(DbContextOptions<SensorDataContext>));

                // Add a database context (SensorDataContext) using an in-memory SQLite database.
                services.AddDbContext<SensorDataContext>(options =>
                {
                    options.UseSqlite(connection);
                });

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database contexts.
                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<SensorDataContext>();

                // Ensure the database is created.
                db.Database.EnsureCreated();

                // add new sensor reading
                db.SensorReadings.Add(new SensorReading { Pressure = 100, Temperature = 20, Humidity = 50, Lux = 300, Uvs = 1, Gas = 10, Timestamp = new System.DateTime(1998, 7, 16) });
            });
        }
    }

}
