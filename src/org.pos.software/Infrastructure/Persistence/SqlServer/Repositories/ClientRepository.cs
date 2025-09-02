using Microsoft.EntityFrameworkCore;
using org.pos.software.Domain.Entities;
using org.pos.software.Domain.OutPort;
using org.pos.software.Infrastructure.Persistence.SqlServer.Mappers;

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

        public async Task<Client?> FindByDni(long dni)
        {
            var entity = await _context.Clients.FirstOrDefaultAsync(x => x.Dni == dni);
            return entity == null ? null : ClientMapper.ToDomain(entity);
        }

        public async Task<Client> Save(Client client)
        {
            var entity = ClientMapper.ToEntity(client);
            _context.Clients.Add(entity);
            await _context.SaveChangesAsync();
            return ClientMapper.ToDomain(entity);
        }

        public Task<Client> Update(Client client)
        {
            throw new NotImplementedException();
        }

    }
}
