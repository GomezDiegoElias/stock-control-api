using org.pos.software.Domain.Entities;
using org.pos.software.Domain.OutPort;

namespace org.pos.software.Infrastructure.Persistence.SqlServer.Repositories
{
    public class ClientRepository : IClientRepository
    {

        private readonly AppDbContext _context;

        public ClientRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<Client> Delete(long dni)
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
