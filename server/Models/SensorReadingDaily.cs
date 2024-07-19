using System.ComponentModel.DataAnnotations;

namespace project_frej.Models
{
    public class SensorReadingDaily
    {
        [Key]
        public int Id { get; set; }  // Primary key

        [Required]
        public DateTime Date { get; set; }

        // Pressure Aggregates
        [Required]
        public double AvgPressure { get; set; }
        [Required]
        public double MinPressure { get; set; }
        [Required]
        public double MaxPressure { get; set; }

        // Temperature Aggregates
        [Required]
        public double AvgTemperature { get; set; }
        [Required]
        public double MinTemperature { get; set; }
        [Required]
        public double MaxTemperature { get; set; }

        // Humidity Aggregates
        [Required]
        public double AvgHumidity { get; set; }
        [Required]
        public double MinHumidity { get; set; }
        [Required]
        public double MaxHumidity { get; set; }
    }
}
