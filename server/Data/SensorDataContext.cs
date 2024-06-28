using Microsoft.EntityFrameworkCore;
using project_frej.Models;

namespace project_frej.Data
{
    public class SensorDataContext : DbContext
    {
        public SensorDataContext(DbContextOptions<SensorDataContext> options)
            : base(options)
        {
        }
        public DbSet<SensorReading> SensorReadings { get; set; }
    }
}
