using Microsoft.EntityFrameworkCore;

namespace FBM.Event.UniqueController.Data.dbEntities
{
    public class UniqueControllerDbContext : DbContext
    {      
        public UniqueControllerDbContext(DbContextOptions options)
          : base(options)
        {
        }
        public DbSet<FBMEventInfo> FBMEventInfos { get; set; }
    }
}
