using Microsoft.EntityFrameworkCore;
using QcOnLocation.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace QcOnLocation.Data
{
    public class LocationContext : DbContext
    {
        public LocationContext(DbContextOptions<LocationContext> options) : base(options) { }

        public DbSet<Location> Locations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Convert string[] to JSON string for storage in SQLite TEXT column
            var options = new JsonSerializerOptions();

            var converter = new ValueConverter<string[]?, string?>(
                v => v == null ? null : JsonSerializer.Serialize(v, options),
                v => v == null ? null : JsonSerializer.Deserialize<string[]>(v, options)
            );

            modelBuilder.Entity<Location>()
                .Property(e => e.Tags)
                .HasConversion(converter)
                .HasColumnType("TEXT");

            modelBuilder.Entity<Location>()
                .Property(e => e.Images)
                .HasConversion(converter)
                .HasColumnType("TEXT");
        }
    }
}