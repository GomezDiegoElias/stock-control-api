using org.pos.software.Domain.Entities;
using org.pos.software.Infrastructure.Persistence.SqlServer.Entities;
using org.pos.software.Infrastructure.Rest.Dto.Response;

namespace org.pos.software.Infrastructure.Persistence.SqlServer.Mappers
{
    public static class UserMapper
    {

        public static User ToDomain(UserEntity entity)
        {
            return new User(entity.Id, entity.FirstName, entity.Country);
        }

        public static UserEntity ToEntity(User domain) 
        {
            return new UserEntity(domain.Id, domain.FirstName, domain.Country);
        }

        public static UserApiResponse ToResponse(User domain)
        {
            return new UserApiResponse(domain.FirstName, domain.Country);
        }

    }
}
