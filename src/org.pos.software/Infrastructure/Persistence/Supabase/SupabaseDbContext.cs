using Microsoft.EntityFrameworkCore;
using org.pos.software.Infrastructure.Persistence.Supabase.Entities;
using Npgsql;

namespace org.pos.software.Infrastructure.Persistence.Supabase
{
    public class SupabaseDbContext : DbContext
    {

        public SupabaseDbContext(DbContextOptions<SupabaseDbContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        //var connectionString = Environment.GetEnvironmentVariable("SUPABASE_CONNECTION_STRING");
        //        var connectionString = "Server=db.[id-proyecto].supabase.co;Port=5432;Database=postgres;User Id=postgres;Password=[tu-contraseña];";
        //        optionsBuilder.UseNpgsql(connectionString);
        //    }
        //}

    }

}
