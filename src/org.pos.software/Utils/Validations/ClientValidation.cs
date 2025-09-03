using FluentValidation;
using org.pos.software.Infrastructure.Rest.Dto.Request;

namespace org.pos.software.Utils.Validations
{
    public class ClientValidation : AbstractValidator<ClientApiRequest>
    {
        public ClientValidation()
        {
            RuleFor(x => x.Dni)
                .NotEmpty().WithMessage("Dni is required")
                .InclusiveBetween(10_000_000, 99_999_999)
                .WithMessage("Dni must be exactly 8 digits");
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");
            RuleFor(x => x.Address)
                .MaximumLength(100).WithMessage("Address cannot exceed 100 characters");
        }
    }
}
