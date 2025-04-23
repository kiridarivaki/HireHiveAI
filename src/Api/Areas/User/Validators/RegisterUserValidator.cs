using Api.Areas.User.Models;
using Application.Validators.Shared;
using FluentValidation;

public class RegisterUserValidator : AbstractValidator<RegisterUserBindingModel>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress();

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First Name is required.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last Name is required.");

        RuleFor(x => x.Password)
            .SetValidator(new PasswordRule());

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Confirmation must match the password.");
    }
}

