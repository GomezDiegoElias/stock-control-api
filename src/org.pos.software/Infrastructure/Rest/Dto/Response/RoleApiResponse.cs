namespace org.pos.software.Infrastructure.Rest.Dto.Response
{
    public record RoleApiResponse
    (
        string Name,
        List<string> Permissions
    );
}
