using org.pos.software.Domain.Entities;
using org.pos.software.Infrastructure.Persistence.SqlServer.Entities;
using org.pos.software.Infrastructure.Rest.Dto.Request;
using org.pos.software.Infrastructure.Rest.Dto.Response;
using org.pos.software.Utils;

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

        // Convierte un UserApiRequest a un User de dominio, generando el hash y la sal de la contraseña
        // Usado en la actualizacion de usuarios
        public static User ToDomain(UserApiRequest request)
        {

            // Genera la sal y el hash de la contraseña
            var salt = PasswordUtils.GenerateRandomSalt();
            var hash = PasswordUtils.HashPasswordWithSalt(request.Password, salt);

            // Parsear el string a Status (con fallbacl si no es valido)
            if (!Enum.TryParse<Status>(request.Status, true, out var status))
            {
                throw new ApplicationException($"Estado '{request.Status}' no es valido. Valores permitidos: ACTIVE, INACTIVE, DELETED.");
            }

            return User.Builder()
                .Dni(request.Dni)
                .Email(request.Email)
                .Salt(salt)
                .Hash(hash)
                .FirstName(request.FirstName)
                .LastName(request.LastName)
                .Role(new Role(request.Role, Enumerable.Empty<string>()))
                .Status(status)
                .Build();
        }

        public static UserApiRequest ToRequest(User domain)
        {
            return new UserApiRequest(
                    domain.Dni,
                    domain.Email,
                    "PROXIMAMENTE", // No se devuelve la contraseña real por seguridad
                    domain.FirstName,
                    domain.LastName ?? string.Empty,
                    domain.Status.ToString(),
                    domain.Role.Name ?? string.Empty
                );
        }

    }
}
