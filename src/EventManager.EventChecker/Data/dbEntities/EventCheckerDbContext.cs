using Microsoft.EntityFrameworkCore;

namespace EventManager.EventChecker.Data.dbEntities
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
