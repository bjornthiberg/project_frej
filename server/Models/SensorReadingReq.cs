using System.ComponentModel.DataAnnotations;

namespace project_frej.Models;

/// <summary>
/// Represents a sensor reading sent to the API.
/// </summary>
public class SensorReadingReq
{
    /// <summary>
    /// Gets or sets the unique identifier for the sensor reading.
    /// </summary>
    [Key]
    public int Id { get; set; }  // Primary key

    /// <summary>
    /// Gets or sets the pressure value of the sensor reading.
    /// </summary>
    [Required]
    public float Pressure { get; set; }

    /// <summary>
    /// Gets or sets the temperature value of the sensor reading.
    /// </summary>
    [Required]
    public float Temperature { get; set; }

    /// <summary>
    /// Gets or sets the humidity value of the sensor reading.
    /// </summary>
    [Required]
    public float Humidity { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the sensor reading was recorded.
    /// </summary>
    [Required]
    public DateTime Timestamp { get; set; }
}
