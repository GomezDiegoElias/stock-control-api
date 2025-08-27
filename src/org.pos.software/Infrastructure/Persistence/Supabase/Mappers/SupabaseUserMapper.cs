using org.pos.software.Domain.Entities;
using org.pos.software.Infrastructure.Persistence.Supabase.Entities;
using org.pos.software.Infrastructure.Rest.Dto.Response;

namespace org.pos.software.Infrastructure.Persistence.Supabase.Mappers
{
    public static class SupabaseUserMapper
    {

        // Expression-bodied member syntax -> traduccion: Miembro con cuerpo de expresión
        //public static User ToDomain(UserEntity entity) => new()
        //{
        //    Id = entity.Id,
        //    FirstName = entity.FirstName,
        //    Country = entity.Country
        //};

        //public static User ToDomain(UserEntity entity)
        //{
        //    return new User(entity.Id, entity.FirstName, entity.Country);
        //}

        //public static UserEntity ToEntity(User domain) 
        //{
        //    return new UserEntity(domain.Id, domain.FirstName, domain.Country);
        //}

        //public static UserApiResponse ToResponse(User domain)
        //{
        //    return new UserApiResponse(domain.FirstName, domain.Country);
        //}

    }
}
