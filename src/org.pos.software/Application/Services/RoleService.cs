using org.pos.software.Application.InPort;
using org.pos.software.Domain.Entities;
using org.pos.software.Domain.OutPort;
using org.pos.software.Infrastructure.Persistence.MySql.Entities;

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
            var entity = await _roleRepository.FindByName(name);
            if (entity == null) return null;
            return new Role(entity.Name, entity.RolePermissions.Select(rp => rp.Permission.Name));
        }

        public async Task<Role> CreateRole(Role role)
        {
            var entity = new RoleEntity
            {
                Name = role.Name
            };
            var saved = await _roleRepository.Save(entity);
            return new Role(saved.Name, saved.RolePermissions.Select(rp => rp.Permission.Name));
        }
    }

}
