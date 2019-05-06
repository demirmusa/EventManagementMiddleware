using EventManager.EventChecker.SQL.Data.DbEntities;
using Microsoft.EntityFrameworkCore;

namespace EventManager.EventChecker.SQL.Data
{
    public class EventCheckerDbContext : DbContext
    {
        public DbSet<EMEventInfo> EMEventInfos { get; set; }

        public EventCheckerDbContext(DbContextOptions options)
          : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<EMEventInfo>()
                .HasIndex(u => u.EventName)
                .IsUnique();
        }
    }
}
