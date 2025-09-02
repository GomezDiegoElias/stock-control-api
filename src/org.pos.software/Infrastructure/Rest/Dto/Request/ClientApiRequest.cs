namespace org.pos.software.Infrastructure.Rest.Dto.Request
{
    public record ClientApiRequest(
        long Dni,
        string FirstName,
        string Address
    ) { }
}
