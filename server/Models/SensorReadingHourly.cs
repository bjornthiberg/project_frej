using System.ComponentModel.DataAnnotations;

namespace project_frej.Models
{
    public class SensorReadingHourly
    {
        [Key]
        public int Id { get; set; }  // Primary key

        [Required]
        public DateTime Hour { get; set; }

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

        // Lux Aggregates
        [Required]
        public double AvgLux { get; set; }
        [Required]
        public double MinLux { get; set; }
        [Required]
        public double MaxLux { get; set; }

        // UV Aggregates
        [Required]
        public double AvgUvs { get; set; }
        [Required]
        public double MinUvs { get; set; }
        [Required]
        public double MaxUvs { get; set; }

        // Gas Aggregates
        [Required]
        public double AvgGas { get; set; }
        [Required]
        public double MinGas { get; set; }
        [Required]
        public double MaxGas { get; set; }
    }
}
