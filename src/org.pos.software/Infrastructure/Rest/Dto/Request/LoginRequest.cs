using System.ComponentModel.DataAnnotations;

namespace org.pos.software.Infrastructure.Rest.Dto.Request
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
