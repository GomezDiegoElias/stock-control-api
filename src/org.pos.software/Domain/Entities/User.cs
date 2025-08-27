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
        public Role Role { get; set; }
        public Status Status { get; set; }

        public User() { }

        public User(string id, long dni, string email, string hash, string salt, string firstName, Role role, Status status)
        {
            Id = id;
            Dni = dni;
            Email = email;
            Hash = hash;
            Salt = salt;
            FirstName = firstName;
            Role = role;
            Status = status;
        }

        // Metodo para generar un ID unico si no se proporciona uno
        public void generateId()
        {
            if (this.Id == null)
            {
                string timestamp = DateTimeOffset.UtcNow.ToString("yyyyMMddHHmmss");
                string uuidPart = Guid.NewGuid().ToString().Split('-')[0];
                this.Id = "usr-" + timestamp + "-" + uuidPart;
            }
        }

        // Patron de diseno Builder
        //public class Builder
        //{

        //    private readonly User _user = new User();

        //    public Builder Id(string id)
        //    {
        //        _user.Id = id;
        //        return this;
        //    }

        //    public Builder Dni(long dni)
        //    {
        //        _user.Dni = dni;
        //        return this;
        //    }

        //    public Builder Email(string email)
        //    {
        //        _user.Email = email;
        //        return this;
        //    }

        //    public Builder Hash(string hash)
        //    {
        //        _user.Hash = hash;
        //        return this;
        //    }

        //    public Builder Salt(string salt)
        //    {
        //        _user.Salt = salt;
        //        return this;
        //    }

        //    public Builder FirstName(string firstName)
        //    {
        //        _user.FirstName = firstName;
        //        return this;
        //    }

        //    public Builder Role(Role role)
        //    {
        //        _user.Role = role;
        //        return this;
        //    }

        //    public Builder Status(Status status)
        //    {
        //        _user.Status = status;
        //        return this;
        //    }

        //    public User Build() => _user;

        //}

    }
}
