namespace org.pos.software.Infrastructure.Rest.Dto.Response
{
    public record ErrorDetails(
            string Message,
            string Details,
            string Path
        ) { }
}
