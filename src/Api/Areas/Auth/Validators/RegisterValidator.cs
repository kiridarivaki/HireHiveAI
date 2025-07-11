﻿using FluentValidation;
using HireHive.Api.Areas.Auth.Models.BindingModels;
using HireHive.Api.Areas.Auth.Validators.Shared;

public class RegisterValidator : AbstractValidator<RegisterBm>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress();

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First Name is required.");

        RuleFor(x => x.LastName)
            .NotEmpty();

        RuleFor(x => x.EmploymentStatus)
            .NotEmpty()
            .IsInEnum()
            .WithMessage("Invalid employment status.");

        RuleFor(x => x.Password)
            .SetValidator(new PasswordRule());

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Confirmation must match the password.");
    }
}

