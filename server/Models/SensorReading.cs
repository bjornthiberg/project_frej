using System.ComponentModel.DataAnnotations;

namespace project_frej.Models
{
    public class SensorReading
    {
        [Key]
        public int Id { get; set; }  // Primary key

        [Required]
        public float Pressure { get; set; }

        [Required]
        public float Temperature { get; set; }

        [Required]
        public float Humidity { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }
    }
}
