using HireHive.Application.Interfaces;
using HireHive.Domain.Exceptions;
using HireHive.Domain.Exceptions.Resume;
using HireHive.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace HireHive.Infrastructure.Jobs
{
    public class ResumeProcessingJob : IResumeProcessingJob
    {
        private readonly IPiiRedactionService _piiRedactionService;
        private readonly IResumeRepository _resumeRepository;
        private readonly ILogger<ResumeProcessingJob> _logger;
        public ResumeProcessingJob(IPiiRedactionService piiRedactionService, IResumeRepository resumeRepository, ILogger<ResumeProcessingJob> logger)
        {
            _piiRedactionService = piiRedactionService;
            _resumeRepository = resumeRepository;
            _logger = logger;
        }
        public async Task ProcessResumeText(Guid userId, byte[] fileBytes)
        {
            try
            {
                var resume = await _resumeRepository.GetByUserIdAsync(userId)
                    ?? throw new ResumeNotFoundException();

                using var fileStream = new MemoryStream(fileBytes);

                var documentText = await _piiRedactionService.ExtractText(fileStream);

                var processedDocumentText = await _piiRedactionService.RedactPii(documentText);
                _logger.LogInformation("Resume processing finished for user {userId}.", userId);

                resume.Update(text: processedDocumentText);
                await _resumeRepository.UpdateAsync(resume);
            }
            catch (ResumeNotFoundException e)
            {
                _logger.LogWarning("Resume of {userId} not found. With exception {message}", userId, e.Message);
                throw;
            }
            catch (BaseException e)
            {
                _logger.LogWarning("Failed to process resume of {userId}. With exception {message}", userId, e.Message);
                throw;
            }
        }
    }
}
