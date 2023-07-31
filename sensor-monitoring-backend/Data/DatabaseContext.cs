using Microsoft.EntityFrameworkCore;
using sensor_monitoring_backend.Domain.Entities;
using sensor_monitoring_backend.Domain.Entity;

namespace sensor_monitoring_backend.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Sensor> Sensors { get; set; }

        public DbSet<PacketDecoder> PacketDecoders { get; set; }
        public DbSet<TemplateSensor> TemplateSensors { get; set; }
        public DbSet<DeterminedValue> DeterminedValues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Device>()
              .HasOne(current => current.User)
              .WithMany(current => current.Devices)
              .HasForeignKey(current => current.UserId);


            modelBuilder.Entity<Sensor>()
               .HasOne(current => current.Device)
               .WithMany(current => current.Sensors)
               .HasForeignKey(current => current.DeviceId);

            modelBuilder.Entity<Sensor>()
               .HasOne(current => current.TemplateSensor)
               .WithMany(current => current.Sensors)
               .HasForeignKey(current => current.TemplateSensorId);


            modelBuilder.Entity<PacketDecoder>()
               .HasOne(current => current.PacketSensor)
               .WithMany(current => current.PacketDecoders)
               .HasForeignKey(current => current.SensorId);


            modelBuilder.Entity<DeterminedValue>()
               .HasOne(current => current.PacketDecoder)
               .WithMany(current => current.DeterminedValues)
               .HasForeignKey(current => current.PacketDecoderId);
        }
    }
}
