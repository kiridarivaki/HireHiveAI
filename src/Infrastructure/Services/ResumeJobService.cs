using HireHive.Application.DTOs.Resume;
using HireHive.Application.Interfaces;
using HireHive.Domain.Entities;
using HireHive.Domain.Exceptions;
using HireHive.Domain.Exceptions.Resume;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HireHive.Infrastructure.Services
{
    public class ResumeJobService : IResumeJobService
    {
        private readonly IPiiRedactionService _piiRedactionService;
        private readonly IResumeService _resumeService;
        private readonly IMapper _mapper;
        private readonly ILogger<ResumeJobService> _logger;
        public ResumeJobService(IPiiRedactionService piiRedactionService, IResumeService resumeService, IMapper mapper, ILogger<ResumeJobService> logger)
        {
            _piiRedactionService = piiRedactionService;
            _resumeService = resumeService;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task ProcessResume(IFormFile file, Guid userId)
        {
            try
            {
                var resume = _mapper.Map<Resume>(await _resumeService.GetByUserId(userId));

                using var ms = new MemoryStream();
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();

                using var fileStream = new MemoryStream(fileBytes);

                var documentText = await _piiRedactionService.ExtractText(fileStream);

                var processedDocumentText = await _piiRedactionService.RedactPii(documentText);

                resume.Update(text: processedDocumentText);
                await _resumeService.Update(resume.Id, _mapper.Map<UpdateResumeDto>(resume));
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
