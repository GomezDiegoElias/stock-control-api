using org.pos.software.Application.InPort;
using org.pos.software.Domain.Entities;
using org.pos.software.Domain.OutPort;
using org.pos.software.Infrastructure.Persistence.MySql.Entities;
using org.pos.software.Infrastructure.Persistence.MySql.Mappers;

namespace org.pos.software.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Role> GetRoleByName(string name)
        {
            var role = await _roleRepository.FindByName(name);
            if (role == null) return null;
            return role;
        }

        public async Task<Role> CreateRole(Role role)
        {
            return await _roleRepository.Save(role);
        }

        public Task<Role> UpdateRolePermissions(string roleName, IEnumerable<string> addPermissions, IEnumerable<string> removePermissions)
        {
            return _roleRepository.UpdatePermissions(roleName, addPermissions, removePermissions);
        }
    }

}
