using org.pos.software.Infrastructure.Persistence.MySql.Entities;

namespace org.pos.software.Domain.OutPort
{
    public interface IRoleRepository
    {
        public Task<RoleEntity> FindByName(string name);
        public Task<RoleEntity> Save(RoleEntity role);
        public Task<List<RoleEntity>> FindAll();
    }
}
