using System.Runtime.Intrinsics.Arm;
using org.pos.software.Domain.Entities;
using org.pos.software.Infrastructure.Persistence.MySql.Entities;
using org.pos.software.Infrastructure.Rest.Dto.Response;

namespace org.pos.software.Infrastructure.Persistence.MySql.Mappers
{
    public static class MySqlUserMapper
    {

        public static User ToDomain(UserEntity entity)
        {
            //return new User(
            //        entity.Id,
            //        entity.Dni,
            //        entity.Email,
            //        entity.Hash,
            //        entity.Salt,
            //        entity.FirstName,
            //        entity.Role,
            //        entity.Status
            //    );
            // Usu del patron de diseno Builder
            return User.Builder()
                .Id(entity.Id)
                .Dni(entity.Dni)
                .Email(entity.Email)
                .Hash(entity.Hash)
                .Salt(entity.Salt)
                .FirstName(entity.FirstName)
                .Role(entity.Role)
                .Status(entity.Status)
                .Build();
        }

        public static UserEntity ToEntity(User domain)
        {
            return new UserEntity(
                    domain.Id,
                    domain.Dni,
                    domain.Email,
                    domain.Hash,
                    domain.Salt,
                    domain.FirstName,
                    domain.Role,
                    domain.Status
                );
        }

        public static UserApiResponse ToResponse(User domain)
        {
            return new UserApiResponse(
                    domain.Id,
                    domain.Dni,
                    domain.Email,
                    domain.FirstName,
                    domain.Role.ToString(),
                    domain.Status.ToString()
                );
        }

    }
}
