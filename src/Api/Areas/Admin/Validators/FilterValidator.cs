using FluentValidation;
using HireHive.Domain.Enums;

namespace HireHive.Api.Areas.Admin.Validators
{
    public class FilterValidator : AbstractValidator<string>
    {
        public FilterValidator()
        {
            RuleFor(f => f)
                .NotEmpty()
                .Must(isValid)
                .WithMessage("Invalid job filter.");
        }
        private bool isValid(string jobPosition)
        {
            return Enum.IsDefined(typeof(JobPositions), jobPosition);
        }
    }
}
