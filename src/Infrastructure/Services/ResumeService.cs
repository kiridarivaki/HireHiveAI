using HireHive.Application.DTOs.Resume;
using HireHive.Application.Interfaces;
using HireHive.Domain.Entities;
using HireHive.Domain.Exceptions.Resume;
using HireHive.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace HireHive.Infrastructure.Services
{
    public class ResumeService : IResumeService
    {
        private readonly IAzureBlobService _azureBlobService;
        private readonly IResumeRepository _resumeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<IResumeService> _logger;
        public ResumeService(IAzureBlobService azureBlobService, IResumeRepository resumeRepository, IMapper mapper, ILogger<IResumeService> logger)
        {
            _azureBlobService = azureBlobService;
            _resumeRepository = resumeRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ResumeDto> GetById(Guid resumeId)
        {
            try
            {
                var resume = await _resumeRepository.GetByIdAsync(resumeId)
                ?? throw new ResumeNotFoundException();

                return _mapper.Map<ResumeDto>(resume);
            }
            catch
            {
                _logger.LogError("Resume {resumeId} not found.", resumeId);
                throw;
            }
        }

        public async Task<UploadResumeDto> Upload(UploadResumeDto uploadDto)
        {
            var file = uploadDto.File;
            var blobName = await _azureBlobService.UploadBlob(file);
            var resume = new Resume(file.FileName, blobName, file.ContentType, file.Length, uploadDto.UserId);

            await _resumeRepository.AddAsync(resume);

            return _mapper.Map<UploadResumeDto>(resume);
        }

        public async Task Update(Guid resumeId, UpdateResumeDto updateResumeDto)
        {
            try
            {
                var resume = await _resumeRepository.GetByIdAsync(resumeId)
                    ?? throw new ResumeNotFoundException();

                var file = updateResumeDto.File;
                var blobName = await _azureBlobService.UploadBlob(file)
                    ?? throw new BlobUploadFailedException();

                resume.Update(file.FileName, blobName, file.ContentType, file.Length);

                await _resumeRepository.UpdateAsync(resume);
                _logger.LogInformation("Resume with id {resumeId} updated.", resumeId);
            }
            catch
            {
                _logger.LogError("Error updating resume {resumeId}.", resumeId);
                throw;
            }
        }

        public async Task Delete(Guid resumeId)
        {
            try
            {
                var resume = await _resumeRepository.GetByIdAsync(resumeId)
                    ?? throw new ResumeNotFoundException();

                await _azureBlobService.DeleteBlob(resume.BlobName!);

                _resumeRepository.Delete(resume);
                _logger.LogInformation("Resume with id {resumeId} deleted.", resumeId);
            }
            catch
            {
                _logger.LogError("Error deleting resume {resumeId}.", resumeId);
                throw;
            }
        }
    }
}
