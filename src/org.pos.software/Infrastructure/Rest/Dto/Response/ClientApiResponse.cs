namespace org.pos.software.Infrastructure.Rest.Dto.Response
{
    public record ClientApiResponse(
        long Dni,
        string FirstName,
        string Address
    ) { }
}
