using Api.Areas.User.Models;
using FluentValidation;


namespace Api.Areas.User.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserBindingModel>
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
