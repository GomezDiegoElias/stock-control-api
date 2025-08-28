using org.pos.software.Domain.Entities;

namespace org.pos.software.Application.InPort
{
    public interface IRoleService
    {
        Task<Role> GetRoleByName(string name);
        Task<Role> CreateRole(Role role);
    }

}
