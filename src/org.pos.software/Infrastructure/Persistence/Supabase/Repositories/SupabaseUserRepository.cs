using org.pos.software.Domain.Entities;

namespace org.pos.software.Infrastructure.Persistence.Supabase.Repositories
{
    public class SupabaseUserRepository
    {

        private readonly SupabaseDbContext _context;

        public SupabaseUserRepository(SupabaseDbContext context)
        {
            _context = context;
        }

        public User FindById(int id)
        {
            var entity = _context.Users.Find(id) ?? throw new Exception("User not found");
            return Mappers.SupabaseUserMapper.ToDomain(entity);
        }

        public List<User> FindAll()
        {
            var entities = _context.Users.ToList();
            return entities.Select(Mappers.SupabaseUserMapper.ToDomain).ToList();
        }

    }
}
