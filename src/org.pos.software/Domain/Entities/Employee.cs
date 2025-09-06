namespace org.pos.software.Domain.Entities
{
    public class Employee
    {

        public string Id { get; set; }
        public long Dni { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string WorkStation { get; set; }
        public bool IsDeleted { get; set; } = false;

        public Employee() { }

        public Employee(string id, long dni, string firstName, string lastName, string workStation)
        {
            Id = id;
            Dni = dni;
            FirstName = firstName;
            LastName = lastName;
            WorkStation = workStation;
        }

        public Employee(long dni, string firstName, string lastName, string workStation)
        {
            Dni = dni;
            FirstName = firstName;
            LastName = lastName;
            WorkStation = workStation;
        }

        public static string GenerateId()
        {
            string timestamp = DateTimeOffset.Now.ToString("yyyyMMddHHmmss");
            string uuidPart = Guid.NewGuid().ToString().Split('-')[0];
            return $"emp-{timestamp}-{uuidPart}";
        }

    }
}
