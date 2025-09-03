using org.pos.software.Application.InPort;
using org.pos.software.Domain.Entities;
using org.pos.software.Domain.OutPort;
using org.pos.software.Infrastructure.Rest.Dto.Response.General;

namespace org.pos.software.Application.Services
{

    public class ClientService : IClientService
    {

        private readonly IClientRepository _repository;

        public ClientService(IClientRepository repository)
        {
            _repository = repository;
        }

        public Task<PaginatedResponse<Client>> FindAll(int pageIndex, int pageSize)
        {
            return _repository.FindAll(pageIndex, pageSize);
        }

        public async Task<Client?> FindByDni(long dni)
        {
            return await _repository.FindByDni(dni);
        }

        public Task<Client> Save(Client client)
        {
            if (string.IsNullOrEmpty(client.Id)) client.Id = Client.GenerateId();
            return _repository.Save(client);
        }

        public async Task<Client> Update(Client client)
        {
            return await _repository.Update(client);
        }

        public async Task<Client> Delete(long id)
        {
            return await _repository.Delete(id);
        }

    }
}
