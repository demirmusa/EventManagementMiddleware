using Microsoft.EntityFrameworkCore;

namespace EventManager.EventChecker.Data.dbEntities
{
    public class EventCheckerDbContext : DbContext
    {      
        public EventCheckerDbContext(DbContextOptions options)
          : base(options)
        {
        }
        public DbSet<EMEventInfo> EMEventInfos { get; set; }
    }
}
