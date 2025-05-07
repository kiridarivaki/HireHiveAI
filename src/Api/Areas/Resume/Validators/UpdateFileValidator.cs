using FluentValidation;
using HireHive.Api.Areas.Resume.Models.BindingModels;
using HireHive.Api.Areas.Resume.Validators.Shared;

namespace HireHive.Api.Areas.Resume.Validators
{
    public class UpdateFileValidator : AbstractValidator<UpdateResumeBm>
    {
        public UpdateFileValidator()
        {
            RuleFor(x => x.File)
                .NotNull().WithMessage("File is required.");
            RuleFor(x => x.File)
                .SetValidator(new SignatureRule());

            RuleFor(x => x.File.Length)
                .LessThanOrEqualTo(5 * 1024 * 1024)
                .When(x => x.File != null)
                .WithMessage("File must be smaller than 5MB.");
        }
    }
}
