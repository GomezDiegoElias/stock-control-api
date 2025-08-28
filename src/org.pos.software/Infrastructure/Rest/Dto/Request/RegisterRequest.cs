namespace org.pos.software.Infrastructure.Rest.Dto.Request
{
    public record RegisterRequest(
        long Dni,
        string Email,
        string Password,
        string FirstName
    ) { }
}
