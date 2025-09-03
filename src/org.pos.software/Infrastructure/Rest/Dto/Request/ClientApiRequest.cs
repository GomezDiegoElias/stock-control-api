namespace org.pos.software.Infrastructure.Rest.Dto.Request
{
    public class ClientApiRequest
    {
        public long Dni { get; set; }
        public string FirstName { get; set; }
        public string Address { get; set; }

        public ClientApiRequest() { }
    }
}
