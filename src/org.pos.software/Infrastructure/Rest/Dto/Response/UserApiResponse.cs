namespace org.pos.software.Infrastructure.Rest.Dto.Response
{
    public record UserApiResponse(
        string Id,
        long Dni, 
        string Email,
        string FirstName,
        string Role,
        string Status
    ) { }
}
