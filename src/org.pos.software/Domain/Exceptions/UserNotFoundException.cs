using System.Net;

namespace org.pos.software.Domain.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string message)
            : base($"Error: {message}") { }
    }
}
