using FluentValidation;

namespace HireHive.Api.Areas.Auth.Validators.Shared
{
    public class PasswordRule : AbstractValidator<string>
    {
        public PasswordRule()
        {
            RuleFor(p => p)
                .NotEmpty()
                .MinimumLength(8)
                .Matches("[A-Z]").WithMessage("Password must contain an uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain a lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain a digit.");
        }
    }
}
