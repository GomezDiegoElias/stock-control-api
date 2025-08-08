using Microsoft.EntityFrameworkCore;
using org.pos.software.Infrastructure.Persistence.MySql.Entities;

namespace org.pos.software.Infrastructure.Persistence.MySql
{
    public class MySqlDbContext : DbContext
    {

        public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; }

    }
}
