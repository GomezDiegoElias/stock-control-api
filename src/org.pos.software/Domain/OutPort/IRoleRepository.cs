using org.pos.software.Domain.Entities;
using org.pos.software.Infrastructure.Persistence.MySql.Entities;

namespace org.pos.software.Domain.OutPort
{
    public interface IRoleRepository
    {
        public Task<Role> FindByName(string name);
        public Task<Role> Save(Role role);
        public Task<List<RoleEntity>> FindAll();
    }
}
