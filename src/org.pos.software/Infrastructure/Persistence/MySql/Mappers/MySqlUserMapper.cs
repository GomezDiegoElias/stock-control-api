using org.pos.software.Domain.Entities;
using org.pos.software.Infrastructure.Persistence.MySql.Entities;
using org.pos.software.Infrastructure.Rest.Dto.Response;

namespace org.pos.software.Infrastructure.Persistence.MySql.Mappers
{
    public static class MySqlUserMapper
    {

        public static User ToDomain(UserEntity entity)
        {
            return new User(entity.Id, entity.FirstName, entity.Country);
        }

        public static UserEntity ToEntity(User user)
        {
            return new UserEntity(user.Id, user.FirstName, user.Country);
        }

        public static UserApiResponse ToResponse(User domain)
        {
            return new UserApiResponse(domain.FirstName, domain.Country);
        }

    }
}
