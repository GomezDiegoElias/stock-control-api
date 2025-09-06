namespace org.pos.software.Infrastructure.Rest.Dto.Request
{
    public record EmployeeApiRequest(
        long Dni,
        string FirstName,
        string LastName,
        string WorkStation
    ) { }
}
