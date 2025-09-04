using org.pos.software.Domain.Entities;

namespace org.pos.software.Domain.OutPort
{
    public interface IRoleRepository
    {
        public Task<Role?> FindByName(string name);
        public Task<Role> Save(Role role);
        public Task<Role> UpdatePermissions(string roleName, IEnumerable<string> addPermissions, IEnumerable<string> removePermissions);
    }
}
