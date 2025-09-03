using org.pos.software.Domain.Entities;
using org.pos.software.Infrastructure.Rest.Dto.Response.General;

namespace org.pos.software.Domain.OutPort
{
    public interface IClientRepository
    {
        public Task<PaginatedResponse<Client>> FindAll(int pageIndex, int pageSize);
        public Task<Client?> FindByDni(long dni);
        public Task<Client> Save(Client client);
        public Task<Client> Update(Client client);
        public Task<Client> Delete(long dni);
        public Task<Client> DeleteLogic(long dni);
    }
}
