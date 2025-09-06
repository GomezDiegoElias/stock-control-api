using org.pos.software.Application.InPort;
using org.pos.software.Domain.Entities;
using org.pos.software.Domain.Exceptions;
using org.pos.software.Domain.OutPort;
using org.pos.software.Infrastructure.Rest.Dto.Response.General;

namespace org.pos.software.Application.Services
{
    public class EmployeeService : IEmployeeService
    {

        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<PaginatedResponse<Employee>> FindAll(int pageIndex, int pageSize, int? dni, string? firstname, string? lastname, string? workstation)
        {
            return await _employeeRepository.FindAll(pageIndex, pageSize, dni, firstname, lastname, workstation);
        }

        public async Task<Employee?> FindByDni(long dni)
        {
            var employee = await _employeeRepository.FindByDni(dni)
                ?? throw new EmployeeNotFoundException(dni);
            return employee;
        }
    }
}
