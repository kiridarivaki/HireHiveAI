using FluentValidation;
using HireHive.Api.Areas.Job.Models.BindingModels;

namespace HireHive.Api.Areas.Job.Validators;

public class JobCriterionValidator : AbstractValidator<JobCriterionBm>
{
    public JobCriterionValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Criterion name is required.");

        RuleFor(x => x.Weight)
            .InclusiveBetween(0, 100).WithMessage("Criterion weight must be between 0 and 100.");
    }
}
