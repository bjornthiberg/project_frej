using System.ComponentModel.DataAnnotations;

namespace project_frej.Models
{
    public class SensorReadingReq
    {
        [Required]
        public float Pressure { get; set; }

        [Required]
        public float Temperature { get; set; }

        [Required]
        public float Humidity { get; set; }

        [Required]
        public float Lux { get; set; }

        [Required]
        public float Uvs { get; set; }

        [Required]
        public float Gas { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }
    }
}
