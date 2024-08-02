using System.ComponentModel.DataAnnotations;

namespace project_frej.Models;

/// <summary>
/// Represents a daily aggregate of sensor readings.
/// </summary>
public class SensorReadingDaily
{
    /// <summary>
    /// Gets or sets the primary key for the aggregate.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the date for which sensor readings were aggregated.
    /// </summary>
    [Required]
    public DateTime Date { get; set; }

    /// <summary>
    /// Gets or sets the average pressure for the day.
    /// </summary>
    [Required]
    public double AvgPressure { get; set; }

    /// <summary>
    /// Gets or sets the minimum pressure for the day.
    /// </summary>
    [Required]
    public double MinPressure { get; set; }

    /// <summary>
    /// Gets or sets the maximum pressure for the day.
    /// </summary>
    [Required]
    public double MaxPressure { get; set; }

    /// <summary>
    /// Gets or sets the average temperature for the day.
    /// </summary>
    [Required]
    public double AvgTemperature { get; set; }

    /// <summary>
    /// Gets or sets the minimum temperature for the day.
    /// </summary>
    [Required]
    public double MinTemperature { get; set; }

    /// <summary>
    /// Gets or sets the maximum temperature for the day.
    /// </summary>
    [Required]
    public double MaxTemperature { get; set; }

    /// <summary>
    /// Gets or sets the average humidity for the day.
    /// </summary>
    [Required]
    public double AvgHumidity { get; set; }

    /// <summary>
    /// Gets or sets the minimum humidity for the day.
    /// </summary>
    [Required]
    public double MinHumidity { get; set; }

    /// <summary>
    /// Gets or sets the maximum humidity for the day.
    /// </summary>
    [Required]
    public double MaxHumidity { get; set; }
}
