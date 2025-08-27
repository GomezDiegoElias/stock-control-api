using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace org.pos.software.Infrastructure.Rest.Dto.Request
{
    public record LoginRequest(
        string Email,
        string Password
    ) { }
}
