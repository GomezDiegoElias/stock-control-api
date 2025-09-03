using org.pos.software.Domain.Entities;
using org.pos.software.Infrastructure.Persistence.SqlServer.Entities;
using org.pos.software.Infrastructure.Rest.Dto.Request;
using org.pos.software.Infrastructure.Rest.Dto.Response;

namespace org.pos.software.Infrastructure.Persistence.SqlServer.Mappers
{
    public static class ClientMapper
    {

        public static Client ToDomain(ClientEntity entity)
        {
            return new Client(entity.Id, entity.Dni, entity.FirstName, entity.Address);
        }

        public static ClientEntity ToEntity(Client domain)
        {
            return new ClientEntity(domain.Id, domain.Dni, domain.FirstName, domain.Address);
        }

        public static ClientApiResponse ToResponse(Client domain)
        {
            return new ClientApiResponse(domain.Dni, domain.FirstName, domain.Address);
        }
        public static Client ToDomain(ClientApiRequest request)
        {
            return new Client(request.Dni, request.FirstName, request.Address);
        }

        public static ClientApiRequest ToRequest(Client domain)
        {
            return new ClientApiRequest
            {
                Dni = domain.Dni,
                FirstName = domain.FirstName,
                Address = domain.Address
            };
        }

    }
}
