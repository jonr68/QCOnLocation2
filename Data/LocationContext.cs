using Microsoft.EntityFrameworkCore;
using QcOnLocation.Models;

namespace QcOnLocation.Data
{
    public class LocationContext : DbContext
    {
        public LocationContext(DbContextOptions<LocationContext> options) : base(options) { }

        public DbSet<Location> Locations { get; set; }
    }
}