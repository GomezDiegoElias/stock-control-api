namespace org.pos.software.Domain.Entities
{
    public class Role
    {

        public string Name { get; set; }
        public List<string> Permissions { get; private set; } = new();

        public Role() { }

        public Role(string name, IEnumerable<string> permissions)
        {
            Name = name;
            Permissions = permissions.ToList();
        }

        public bool HasPermission(string permission) => Permissions.Contains(permission);

        // Roles predefinidos
        public static Role ADMIN => new Role("ADMIN", new[] { "CREATE", "READ", "UPDATE", "DELETE" });
        public static Role PRESUPUESTISTA => new Role("PRESUPUESTISTA", new[] { "CREATE_ORDER", "READ_ORDER" });
        public static Role USER => new Role("USER", new[] { "READ_ORDER" });

    }
}
