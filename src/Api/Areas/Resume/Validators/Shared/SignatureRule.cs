using FileSignatures;
using FluentValidation;

namespace HireHive.Api.Areas.Resume.Validators.Shared
{
    public class SignatureRule : AbstractValidator<IFormFile>
    {
        public SignatureRule()
        {
            RuleFor(f => f)
                .Must(hasValidSignature).WithMessage("File is not in PDF format.");
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
