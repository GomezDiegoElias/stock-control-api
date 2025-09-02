namespace org.pos.software.Domain.Entities
{
    public class Client
    {

        public string Id { get; set; }
        public long Dni { get; set; }
        public string FirstName { get; set; }
        public string Address { get; set; }

        public Client() { }

        public Client(string id, long dni, string firstname, string address)
        {
            Id = id;
            Dni = dni;
            FirstName = firstname;
            Address = address;
        }

        public Client(long dni, string firstname, string address)
        {
            Dni = dni;
            FirstName = firstname;
            Address = address;
        }

        public static string GenerateId()
        {
            string timestap = DateTimeOffset.UtcNow.ToString("yyyyMMddHHmmss");
            string uuidPart = Guid.NewGuid().ToString().Split('-')[0];
            return $"cli-{timestap}-{uuidPart}";
        }

    }
}
