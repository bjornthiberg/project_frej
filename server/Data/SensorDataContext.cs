using Microsoft.EntityFrameworkCore;
using project_frej.Models;

namespace project_frej.Data;

/// <summary>
/// Represents the database context for sensor data.
/// Primary constructor Initializes a new instance of the <see cref="SensorDataContext"/> class with the specified options.
/// </summary>
/// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
public class SensorDataContext(DbContextOptions<SensorDataContext> options) : DbContext(options)
{
    /// <summary>
    /// Gets or sets the DbSet for sensor readings.
    /// </summary>
    public DbSet<SensorReading> SensorReadings { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for hourly sensor readings.
    /// </summary>
    public DbSet<SensorReadingHourly> SensorReadingsHourly { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for daily sensor readings.
    /// </summary>
    public DbSet<SensorReadingDaily> SensorReadingsDaily { get; set; }
}
