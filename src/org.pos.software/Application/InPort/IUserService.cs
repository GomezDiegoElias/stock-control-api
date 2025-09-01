using org.pos.software.Domain.Entities;
using org.pos.software.Infrastructure.Rest.Dto.Response.General;

namespace org.pos.software.Application.Ports
{
    public interface IUserService
    {
        public Task<PaginatedResponse<User>> FindAllUsers(int pageIndex, int pageSize);
        public Task<User?> FindByDni(long dni);
    }
}
