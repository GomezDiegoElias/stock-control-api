namespace org.pos.software.Infrastructure.Rest.Dto.Response.General
{
    public record ErrorDetails(
            string Message,
            string Details,
            string Path
        ) { }
}
