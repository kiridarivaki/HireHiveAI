using HireHive.Application.Interfaces;
using HireHive.Domain.Exceptions.Resume;
using HireHive.Domain.Interfaces;

namespace HireHive.Infrastructure.Services
{
    public class ResumeJobService : IResumeJobService
    {
        private readonly IPiiRedactionService _piiRedactionService;
        private readonly IResumeRepository _resumeRepository;
        public ResumeJobService(IPiiRedactionService piiRedactionService, IResumeRepository resumeRepository)
        {
            _piiRedactionService = piiRedactionService;
            _resumeRepository = resumeRepository;
        }
        public async Task ProcessResume(byte[] fileBytes, Guid userId)
        {
            var resume = await _resumeRepository.GetByUserIdAsync(userId)
                ?? throw new ResumeNotFoundException();

            using var fileStream = new MemoryStream(fileBytes);

            var documentText = await _piiRedactionService.ExtractText(fileStream);

            var processedDocumentText = await _piiRedactionService.RedactPii(documentText);

            resume.Update(text: processedDocumentText);
            await _resumeRepository.UpdateAsync(resume);
        }
    }
}
