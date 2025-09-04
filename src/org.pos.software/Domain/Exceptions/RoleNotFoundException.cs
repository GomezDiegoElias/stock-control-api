namespace org.pos.software.Domain.Exceptions
{
    public class RoleNotFoundException : Exception
    {
        public RoleNotFoundException(string roleName)
            : base($"Rol '{roleName}' no existe.") { }
    }
}
