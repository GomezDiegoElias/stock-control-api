using org.pos.software.Domain.Entities;

namespace org.pos.software.Domain.OutPort
{
    public interface IClientRepository
    {
        public Task<Client> FindAll(int pageIndex, int pageSize);
        public Task<Client?> FindByDni(long dni);
        public Task<Client> Save(Client client);
        public Task<Client> Update(Client client);
        public Task<Client> Delete(long dni);
    }
}
