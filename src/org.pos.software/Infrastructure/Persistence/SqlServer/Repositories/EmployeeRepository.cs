using org.pos.software.Domain.Entities;
using org.pos.software.Domain.OutPort;
using org.pos.software.Infrastructure.Rest.Dto.Response.General;

namespace org.pos.software.Infrastructure.Persistence.SqlServer.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {

        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResponse<Employee>> FindAll(int pageIndex, int pageSize)
        {
            return await _context.getEmployeePagination(pageIndex, pageSize);
        }
    }
}
