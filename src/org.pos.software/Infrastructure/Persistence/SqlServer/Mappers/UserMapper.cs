using org.pos.software.Domain.Entities;
using org.pos.software.Infrastructure.Persistence.SqlServer.Entities;
using org.pos.software.Infrastructure.Rest.Dto.Response;

namespace org.pos.software.Infrastructure.Persistence.SqlServer.Mappers
{
    public static class UserMapper
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
                .LastName(entity.LastName)
                .Status(entity.Status)
                //.Role(new Role(entity.Role.Name, entity.Role.RolePermissions.Select(rp => rp.Permission.Name) ?? Enumerable.Empty<string>())) // asigna rol con permisos
                .Role(new Role(entity.Role.Name, entity.Role.RolePermissions.Select(rp => rp.Permission.Name)))
                .Build();
        }

        public static UserEntity toEntity(User domain, RoleEntity roleEntity)
        {
            return new UserEntity(
                domain.Id,
                domain.Dni,
                domain.Email,
                domain.Hash,
                domain.Salt,
                domain.FirstName,
                domain.LastName,
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
                    domain.LastName ?? string.Empty,
                    domain.Role.Name ?? string.Empty,
                    domain.Status.ToString()
                );
        }

    }
}
