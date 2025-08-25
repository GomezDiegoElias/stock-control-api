namespace org.pos.software.Infrastructure.Rest.Dto.Response
{
    public record StandardResponse<T>(
            bool Success,
            string Message,
            T? Data = default,
            ErrorDetails? Error = null,
            int Status = 200
        ) { }
}
