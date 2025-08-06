using Microsoft.EntityFrameworkCore;
using org.pos.software.Infrastructure.Persistence.Entities;

namespace org.pos.software.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; }

    }
}
