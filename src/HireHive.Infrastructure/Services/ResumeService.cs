using Hangfire;
using HireHive.Application.DTOs.Resume;
using HireHive.Application.Interfaces;
using HireHive.Domain.Entities;
using HireHive.Domain.Exceptions;
using HireHive.Domain.Exceptions.Resume;
using HireHive.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Transactions;

namespace HireHive.Infrastructure.Services
{
    public class ResumeService : IResumeService
    {
        private readonly IBlobService _azureBlobService;
        private readonly IResumeRepository _resumeRepository;
        private readonly IResumeProcessingJob _resumeJobService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IMapper _mapper;
        private readonly ILogger<ResumeService> _logger;
        public ResumeService(
            IBlobService azureBlobService,
            IResumeRepository resumeRepository,
            IResumeProcessingJob resumeJobService,
            IBackgroundJobClient backgroundJobClient,
            IMapper mapper,
            ILogger<ResumeService> logger)
        {
            _azureBlobService = azureBlobService;
            _resumeRepository = resumeRepository;
            _resumeJobService = resumeJobService;
            _backgroundJobClient = backgroundJobClient;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ResumeDto> GetByUserId(Guid userId)
        {
            try
            {
                var resume = await _resumeRepository.GetByUserIdAsync(userId);
                _logger.LogInformation("Resume of user {resumeId} fetched.", userId);

                return _mapper.Map<ResumeDto>(resume);
            }
            catch (BaseException e)
            {
                _logger.LogWarning("Resume {resumeId} not found. With exception {message}", userId, e.Message);
                throw;
            }
        }

        public async Task<UploadResumeDto> Upload(Guid userId, UploadResumeDto uploadDto)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                                             new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                             TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var file = uploadDto.File;
                    var blobName = await _azureBlobService.Upload(file);

                    var resume = new Resume(file.FileName, blobName, file.ContentType, file.Length, userId, null);

                    await _resumeRepository.AddAsync(resume);

                    var fileBytes = GetFileBytes(uploadDto.File);

                    var fileProcessingJob = _backgroundJobClient.Enqueue(() => _resumeJobService.ProcessResumeText(userId, fileBytes));

                    scope.Complete();

                    _logger.LogInformation("Resume with id {resumeId} uploaded.", resume.Id);
                    return _mapper.Map<UploadResumeDto>(resume);

                }
                catch (BaseException e)
                {
                    _logger.LogWarning("Failed to upload resume for customer {userId}. With exception: {message}", userId, e.Message);
                    throw;
                }
            }
        }

        public async Task Update(Guid userId, UpdateResumeDto updateDto)
        {
            try
            {
                var resume = await _resumeRepository.GetByUserIdAsync(userId)
                    ?? throw new ResumeNotFoundException();

                await _azureBlobService.Delete(resume.BlobName!);

                var file = updateDto.File;
                var blobName = await _azureBlobService.Upload(file);

                resume.Update(file.FileName, blobName, file.ContentType, file.Length, null);

                await _resumeRepository.UpdateAsync(resume);

                var fileBytes = GetFileBytes(updateDto.File);

                var fileProcessingJob = _backgroundJobClient.Enqueue(() => _resumeJobService.ProcessResumeText(userId, fileBytes));

                _logger.LogInformation("Resume with id {resumeId} updated.", resume.Id);
            }
            catch (BaseException e)
            {
                _logger.LogWarning("Failed to update resume for user {userId}. With exception: {message}", userId, e.Message);
                throw;
            }
        }

        public async Task Delete(Guid userId)
        {
            try
            {
                var resume = await _resumeRepository.GetByUserIdAsync(userId)
                    ?? throw new ResumeNotFoundException();

                await _azureBlobService.Delete(resume.BlobName!);

                _resumeRepository.DeleteAsync(resume);
                _logger.LogInformation("Resume of user {userId} deleted.", userId);
            }
            catch (BaseException e)
            {
                _logger.LogWarning("Failed to delete resume of user {userId}. With exception: {message}", userId, e.Message);
                throw;
            }
        }

        public byte[] GetFileBytes(IFormFile file)
        {
            using var ms = new MemoryStream();

            file.CopyTo(ms);
            var fileBytes = ms.ToArray();

            return fileBytes;
        }
    }
}
