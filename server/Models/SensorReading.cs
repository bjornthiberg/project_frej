namespace project_frej.Models
{
    public class SensorReading
    {
        public float Pressure { get; set; }
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public float Lux { get; set; }
        public float Uvs { get; set; }
        public float Gas { get; set; }
        public DateTime Timestamp { get; set; }
    }

}
