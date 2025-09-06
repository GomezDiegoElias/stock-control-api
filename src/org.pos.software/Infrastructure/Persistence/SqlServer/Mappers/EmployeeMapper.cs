using org.pos.software.Domain.Entities;
using org.pos.software.Infrastructure.Persistence.SqlServer.Entities;
using org.pos.software.Infrastructure.Rest.Dto.Request;
using org.pos.software.Infrastructure.Rest.Dto.Response;

namespace org.pos.software.Infrastructure.Persistence.SqlServer.Mappers
{
    public static class EmployeeMapper
    {

        public static EmployeeEntity ToEntity(Employee domain)
        {
            return new EmployeeEntity
            {
                Id = domain.Id,
                Dni = domain.Dni,
                FirstName = domain.FirstName,
                LastName = domain.LastName,
                WorkStation = domain.WorkStation,
                IsDeleted = domain.IsDeleted
            };
        }

        public static Employee ToDomain(EmployeeEntity entity)
        {
            return new Employee
            {
                Id = entity.Id,
                Dni = entity.Dni,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                WorkStation = entity.WorkStation,
                IsDeleted = entity.IsDeleted
            };
        }

        public static Employee ToDomain(EmployeeApiRequest request)
        {
            return new Employee
            {
                Id = Employee.GenerateId(),
                Dni = request.Dni,
                FirstName = request.FirstName,
                LastName = request.LastName,
                WorkStation = request.WorkStation,
                IsDeleted = false
            };
        }

        public static EmployeeApiRequest ToRequest(Employee domain)
        {
            return new EmployeeApiRequest(
                domain.Dni,
                domain.FirstName,
                domain.LastName,
                domain.WorkStation
            );
        }

        public static EmployeeApiResponse ToResponse(Employee domain)
        {
            return new EmployeeApiResponse(
                domain.Dni,
                domain.FirstName,
                domain.LastName,
                domain.WorkStation
            );
        }

    }
}
