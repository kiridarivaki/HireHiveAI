using FileSignatures;
using FluentValidation;
using HireHive.Api.Areas.Resume.Models;

namespace HireHive.Api.Areas.Resume.Validators
{
    public class FileValidator : AbstractValidator<UploadResumeBm>
    {
        public FileValidator()
        {
            RuleFor(x => x.File)
                .NotNull().WithMessage("File is required.");

            RuleFor(x => x.File)
                .Must(hasValidSignature).WithMessage("File is not in PDF format.");

            RuleFor(x => x.File.Length)
                .LessThanOrEqualTo(5 * 1024 * 1024)
                .When(x => x.File != null)
                .WithMessage("File must be smaller than 5MB.");
        }

        public bool hasValidSignature(IFormFile file)
        {
            var inspector = new FileFormatInspector();

            using (var stream = file.OpenReadStream())
            {
                var format = inspector.DetermineFileFormat(stream);

                byte[] pdfMagicBytes = "%PDF"u8.ToArray();

                bool isPdf = format != null &&
                       format.Signature != null &&
                       format.Signature.Take(pdfMagicBytes.Length).SequenceEqual(pdfMagicBytes);

                return isPdf;
            }
        }
    }
}
