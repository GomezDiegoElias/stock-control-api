using Microsoft.EntityFrameworkCore;
using org.pos.software.Domain.Entities;
using org.pos.software.Domain.Exceptions;
using org.pos.software.Domain.OutPort;
using org.pos.software.Infrastructure.Persistence.SqlServer.Mappers;
using org.pos.software.Infrastructure.Rest.Dto.Response.General;

namespace org.pos.software.Infrastructure.Persistence.SqlServer.Repositories
{
    public class ClientRepository : IClientRepository
    {

        private readonly AppDbContext _context;

        public ClientRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Client> Delete(long dni)
        {
            
            var entity = _context.Clients.FirstOrDefault(x => x.Dni == dni);

            if (entity == null) throw new ClientNotFoundException($"No se encontró cliente con DNI {dni}");

            _context.Clients.Remove(entity);
            await _context.SaveChangesAsync();

            return ClientMapper.ToDomain(entity);

        }

        public async Task<PaginatedResponse<Client>> FindAll(int pageIndex, int pageSize)
        {
            return await _context.getClientPagination(pageIndex, pageSize);
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

        public async Task<Client> Update(Client client)
        {
            var existingEntity = await _context.Clients.FirstOrDefaultAsync(c => c.Id == client.Id);
            if (existingEntity == null)
                throw new ClientNotFoundException($"No se encontró cliente con Id {client.Id}");

            // Actualizar solo los campos necesarios
            existingEntity.Dni = client.Dni;
            existingEntity.FirstName = client.FirstName;
            existingEntity.Address = client.Address;
            existingEntity.UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") != null ? DateTime.Now : DateTime.MinValue;

            await _context.SaveChangesAsync();
            return ClientMapper.ToDomain(existingEntity);
        }


    }
}
