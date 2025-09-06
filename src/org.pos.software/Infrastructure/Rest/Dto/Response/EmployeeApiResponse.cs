namespace org.pos.software.Infrastructure.Rest.Dto.Response
{
    public record EmployeeApiResponse(
        long Dni,
        string FirstName,
        string LastName,
        string WorkStation
    ) { }
}
