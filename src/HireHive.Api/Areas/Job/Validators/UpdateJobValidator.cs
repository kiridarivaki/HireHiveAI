using FluentValidation;
using HireHive.Api.Areas.Job.Models.BindingModels;

namespace HireHive.Api.Areas.Job.Validators;

public class UpdateJobValidator : AbstractValidator<UpdateJobBm>
{
    public UpdateJobValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.")
            .When(x => x.Title != null);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .When(x => x.Description != null);

        RuleFor(x => x.JobType)
            .IsInEnum().WithMessage("Job type is invalid.")
            .When(x => x.JobType != null);

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status is invalid.")
            .When(x => x.Status != null);

        RuleForEach(x => x.Criteria)
            .SetValidator(new JobCriterionValidator())
            .When(x => x.Criteria != null);
    }
}
