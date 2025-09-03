namespace org.pos.software.Domain.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string dni)
            : base($"Usuario con DNI {dni} no existe") { }
    }
}
