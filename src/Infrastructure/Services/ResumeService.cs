using HireHive.Application.DTOs.Resume;
using HireHive.Application.Interfaces;
using HireHive.Domain.Entities;
using HireHive.Domain.Exceptions;
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
        private readonly ILogger<ResumeService> _logger;
        public ResumeService(
            IAzureBlobService azureBlobService,
            IResumeRepository resumeRepository,
            IMapper mapper,
            ILogger<ResumeService> logger)
        {
            _azureBlobService = azureBlobService;
            _resumeRepository = resumeRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ResumeDto> GetByUserId(Guid userId)
        {
            try
            {
                var resume = await _resumeRepository.GetByUserIdAsync(userId)
                    ?? throw new ResumeNotFoundException();

                return _mapper.Map<ResumeDto>(resume);
            }
            catch (ResumeNotFoundException)
            {
                _logger.LogError("Resume {resumeId} not found.", userId);
                throw;
            }
        }

        public async Task<UploadResumeDto> Upload(UploadResumeDto uploadDto)
        {
            try
            {
                var file = uploadDto.File;
                var blobName = await _azureBlobService.UploadBlob(file);

                var resume = new Resume(file.FileName, blobName, file.ContentType, file.Length, uploadDto.UserId);

                await _resumeRepository.AddAsync(resume);

                return _mapper.Map<UploadResumeDto>(resume);
            }
            catch (BaseException)
            {
                _logger.LogError("Resume upload failed for customer {userId}", uploadDto.UserId);
                throw;
            }
        }

        public async Task Update(Guid userId, UpdateResumeDto updateResumeDto)
        {
            try
            {
                var resume = await _resumeRepository.GetByUserIdAsync(userId)
                    ?? throw new ResumeNotFoundException();

                await _azureBlobService.DeleteBlob(resume.BlobName!);

                var file = updateResumeDto.File;
                var blobName = await _azureBlobService.UploadBlob(file)
                    ?? throw new BlobUploadFailedException();

                resume.Update(file.FileName, blobName, file.ContentType, file.Length);

                await _resumeRepository.UpdateAsync(resume);
                _logger.LogInformation("Resume with id {resumeId} updated.", resume.Id);
            }
            catch (ResumeNotFoundException)
            {
                _logger.LogError("Resume for user {userId} not found.", userId);
                throw;
            }
            catch (BlobUploadFailedException)
            {
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
            catch (ResumeNotFoundException)
            {
                _logger.LogError("Resume {resumeId} not found.", resumeId);
                throw;
            }
            catch (BlobUploadFailedException)
            {
                throw;
            }
        }
    }
}
