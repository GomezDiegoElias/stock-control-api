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

        public Task<Client> FindByDni(long dni)
        {
            throw new NotImplementedException();
        }

        public Task<Client> Save(Client client)
        {
            throw new NotImplementedException();
        }

        public Task<Client> Update(Client client)
        {
            throw new NotImplementedException();
        }

    }
}
