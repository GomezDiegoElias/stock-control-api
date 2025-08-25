using Microsoft.EntityFrameworkCore;
using org.pos.software.Infrastructure.Persistence.SqlServer.Entities;

namespace org.pos.software.Infrastructure.Persistence.SqlServer
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; }

    }
}
