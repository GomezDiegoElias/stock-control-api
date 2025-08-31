namespace org.pos.software.Domain.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string dni)
            : base($"User whit DNI {dni} not found") { }
    }
}
