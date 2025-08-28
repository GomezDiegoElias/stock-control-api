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
            return User.Builder()
                .Id(entity.Id)
                .Dni(entity.Dni)
                .Email(entity.Email)
                .Hash(entity.Hash)
                .Salt(entity.Salt)
                .FirstName(entity.FirstName)
                .Status(entity.Status)
                .Role(new Role(entity.Role.Name, entity.Role.RolePermissions.Select(rp => rp.Permission.Name))) // asigna rol con permisos
                .Build();
        }

        public static UserEntity ToEntity(User domain, RoleEntity roleEntity)
        {
            return new UserEntity(
                domain.Id,
                domain.Dni,
                domain.Email,
                domain.Hash,
                domain.Salt,
                domain.FirstName,
                domain.Status,
                roleEntity.Id
            )
            {
                Role = roleEntity
            };
        }

        public static UserApiResponse ToResponse(User domain)
        {
            return new UserApiResponse(
                    domain.Id,
                    domain.Dni,
                    domain.Email,
                    domain.FirstName,
                    domain.Role.Name ?? string.Empty,
                    domain.Status.ToString()
                );
        }

        //public static UserApiResponse ToResponse(UserEntity entity)
        //{
        //    return new UserApiResponse(
        //            entity.Id,
        //            entity.Dni,
        //            entity.Email,
        //            entity.FirstName,
        //            entity.Role.Name ?? string.Empty,
        //            entity.Status.ToString()
        //        );
        //}

    }
}
