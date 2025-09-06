namespace org.pos.software.Domain.Exceptions
{
    public class EmployeeNotFoundException : Exception
    {
        public EmployeeNotFoundException(long dni)
            : base($"Employee with DNI {dni} not found.") { }
    }
}
