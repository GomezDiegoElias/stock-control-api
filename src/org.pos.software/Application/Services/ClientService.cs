using org.pos.software.Application.InPort;
using org.pos.software.Domain.Entities;
using org.pos.software.Domain.OutPort;

namespace org.pos.software.Application.Services
{

    public class ClientService : IClientService
    {

        private readonly IClientRepository _repository;

        public ClientService(IClientRepository repository)
        {
            _repository = repository;
        }

        public Task<Client> Delete(long id)
        {
            throw new NotImplementedException();
        }

        public Task<Client> FindAll(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
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

        public Task<Client> Update(Client client)
        {
            throw new NotImplementedException();
        }

    }
}
