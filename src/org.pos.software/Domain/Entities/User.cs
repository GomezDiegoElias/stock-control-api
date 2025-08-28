using org.pos.software.Utils.Patterns;

namespace org.pos.software.Domain.Entities
{
    public class User
    {

        public static UserBuilder Builder() => new UserBuilder();

        public string Id { get; set; }
        public long Dni { get; set; }
        public string Email { get; set; }
        public string Hash { get; set; }
        public string Salt { get; set; }
        public string FirstName { get; set; }
        public Status Status { get; set; }

        public Role Role { get; set; }

        public User() { }

        public User(string id, long dni, string email, string hash, string salt, string firstName, Status status)
        {
            Id = string.IsNullOrEmpty(id) ? GenerateId() : id;
            Dni = dni;
            Email = email;
            Hash = hash;
            Salt = salt;
            FirstName = firstName;
            Status = status;
        }

        public bool Can(string permission) => Role != null && Role.HasPermission(permission);

        // Metodo para generar un ID unico si no se proporciona uno
        public static string GenerateId()
        {
            string timestamp = DateTimeOffset.UtcNow.ToString("yyyyMMddHHmmss");
            string uuidPart = Guid.NewGuid().ToString().Split('-')[0];
            return $"usr-{timestamp}-{uuidPart}";
        }

    }
}
