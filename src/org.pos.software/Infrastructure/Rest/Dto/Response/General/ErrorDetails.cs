namespace org.pos.software.Infrastructure.Rest.Dto.Response.General
{
    public record ErrorDetails(
            int StatusCode,
            string Message,
            string Path,
            string? Details
        ) { }
}
