using org.pos.software.Domain.Entities;
using org.pos.software.Infrastructure.Rest.Dto.Response.General;

namespace org.pos.software.Application.InPort
{
    public interface IEmployeeService
    {
        public Task<PaginatedResponse<Employee>> FindAll(int pageIndex, int pageSize);
    }
}
