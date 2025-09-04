using org.pos.software.Domain.Entities;
using org.pos.software.Infrastructure.Rest.Dto.Response.General;

namespace org.pos.software.Domain.OutPort
{
    public interface IUserRepository
    {
        public Task<PaginatedResponse<User>> FindAll(int pageIndex, int pageSize);
        public Task<User?> FindByDni(long dni);
        public Task<User> Save(User user);
        public Task<User?> FindByEmail(string email);
        public Task<User> DeletePermanent(long dni);
        public Task<User> DeleteLogic(long dni);
        public Task<User> Update(User user);
    }
}
