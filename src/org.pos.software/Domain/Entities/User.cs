namespace org.pos.software.Domain.Entities
{
    public class User
    {
        
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Country { get; set; }

        public User() { }

        public User(int id, string firstName, string country)
        {
            Id = id;
            FirstName = firstName;
            Country = country;
        }

    }
}
