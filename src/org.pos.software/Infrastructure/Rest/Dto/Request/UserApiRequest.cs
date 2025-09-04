namespace org.pos.software.Infrastructure.Rest.Dto.Request
{
    public record UserApiRequest(
        long Dni,
        string Email,
        string Password,
        string FirstName,
        string LastName,
        string Status,
        string Role
    ) { }
}
