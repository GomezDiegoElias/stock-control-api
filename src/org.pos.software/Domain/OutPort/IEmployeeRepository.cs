using org.pos.software.Domain.Entities;
using org.pos.software.Infrastructure.Rest.Dto.Response.General;

namespace org.pos.software.Domain.OutPort
{
    public interface IEmployeeRepository
    {
        public Task<PaginatedResponse<Employee>> FindAll(int pageIndex, int pageSize, int? dni, string? firstname, string? lastname, string? workstation);
        public Task<Employee?> FindByDni(long dni);
    }
}
