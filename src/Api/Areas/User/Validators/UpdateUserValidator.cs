using FluentValidation;
using HireHive.Api.Areas.User.Models;


namespace HireHive.Api.Areas.User.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateBm>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required.")
                .NotEmpty().When(x => x.FirstName != null);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .When(x => x.LastName != null);
        }
    }
}
