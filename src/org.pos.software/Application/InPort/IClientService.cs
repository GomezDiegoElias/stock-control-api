using Microsoft.AspNetCore.JsonPatch;
using org.pos.software.Domain.Entities;
using org.pos.software.Infrastructure.Rest.Dto.Response.General;

namespace org.pos.software.Application.InPort
{
    public interface IClientService
    {
        public Task<PaginatedResponse<Client>> FindAll(int pageIndex, int pageSize);
        public Task<Client?> FindByDni(long dni);
        public Task<Client> Save(Client client);
        public Task<Client> Update(Client client);
        public Task<Client> Delete(long id);
        public Task<Client> DeleteLogic(long dni);
        public Task<Client> UpdatePartial(long dni, JsonPatchDocument<Client> patchDoc);
    }
}
