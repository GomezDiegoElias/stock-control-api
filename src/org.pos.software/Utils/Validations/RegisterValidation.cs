using FluentValidation;
using org.pos.software.Infrastructure.Rest.Dto.Request;

namespace org.pos.software.Utils.Validations
{
    public class RegisterValidation : AbstractValidator<RegisterRequest>
    {

        public RegisterValidation() {
            RuleFor(x => x.Dni)
                .NotEmpty().WithMessage("DNI is required.")
                .GreaterThan(0).WithMessage("DNI must be a positive number.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");
        }

    }
}
