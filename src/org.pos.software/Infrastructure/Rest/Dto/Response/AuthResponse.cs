namespace org.pos.software.Infrastructure.Rest.Dto.Response
{
    public record AuthResponse(
        string Message,
        string AccessToken,
        string RefreshToken
    ) { }
}
