using org.pos.software.Domain.Entities;

namespace org.pos.software.Utils.Patterns
{
    public class UserBuilder : Builder<UserBuilder, User>
    {
        public UserBuilder Id(string id)
        {
            _entity.Id = id;
            return This;
        }

        public UserBuilder Dni(long dni)
        {
            _entity.Dni = dni;
            return This;
        }

        public UserBuilder Email(string email)
        {
            _entity.Email = email;
            return This;
        }

        public UserBuilder Hash(string hash)
        {
            _entity.Hash = hash;
            return This;
        }

        public UserBuilder Salt(string salt)
        {
            _entity.Salt = salt;
            return This;
        }

        public UserBuilder FirstName(string firstName)
        {
            _entity.FirstName = firstName;
            return This;
        }

        public UserBuilder Status(Status status)
        {
            _entity.Status = status;
            return This;
        }

        public UserBuilder Role(Role role)
        {
            _entity.Role = role;
            return This;
        }

    }

}
