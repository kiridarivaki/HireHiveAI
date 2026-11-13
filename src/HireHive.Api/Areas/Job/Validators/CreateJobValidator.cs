using FluentValidation;
using HireHive.Api.Areas.Job.Models.BindingModels;

namespace HireHive.Api.Areas.Job.Validators;

public class CreateJobValidator : AbstractValidator<CreateJobBm>
{
    public CreateJobValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.JobType)
            .IsInEnum().WithMessage("Job type is invalid.");

        RuleForEach(x => x.Criteria)
            .SetValidator(new JobCriterionValidator());
    }
}
