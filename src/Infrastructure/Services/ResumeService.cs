using HireHive.Application.DTOs.Resume;
using HireHive.Application.Interfaces;
using HireHive.Domain.Entities;
using HireHive.Domain.Exceptions;
using HireHive.Domain.Exceptions.Resume;
using HireHive.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Transactions;

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
            catch (BaseException e)
            {
                _logger.LogWarning("Resume {resumeId} not found. With exception {message}", userId, e.Message);
                throw;
            }
        }

        public async Task<UploadResumeDto> Upload(UploadResumeDto uploadDto)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                                             new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                             TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var file = uploadDto.File;
                    var blobName = await _azureBlobService.Upload(file);

                    var resume = new Resume(file.FileName, blobName, file.ContentType, file.Length, uploadDto.UserId);

                    await _resumeRepository.AddAsync(resume);
                    scope.Complete();

                    return _mapper.Map<UploadResumeDto>(resume);

                }
                catch (BaseException e)
                {
                    _logger.LogError("Failed to upload resume for customer {userId}. With exception: {message}", uploadDto.UserId, e.Message);
                    throw;
                }
            }
        }

        public async Task Update(Guid userId, UpdateResumeDto updateResumeDto)
        {
            try
            {
                var resume = await _resumeRepository.GetByUserIdAsync(userId)
                    ?? throw new ResumeNotFoundException();

                await _azureBlobService.Delete(resume.BlobName!);

                var file = updateResumeDto.File;
                var blobName = await _azureBlobService.Upload(file);


                resume.Update(file.FileName, blobName, file.ContentType, file.Length, null);

                await _resumeRepository.UpdateAsync(resume);
                _logger.LogInformation("Resume with id {resumeId} updated.", resume.Id);
            }
            catch (BaseException e)
            {
                _logger.LogWarning("Failed to update resume for user {userId}. With exception: {message}", userId, e.Message);
                throw;
            }
        }

        public async Task Delete(Guid resumeId)
        {
            try
            {
                var resume = await _resumeRepository.GetByIdAsync(resumeId)
                    ?? throw new ResumeNotFoundException();

                await _azureBlobService.Delete(resume.BlobName!);

                _resumeRepository.Delete(resume);
                _logger.LogInformation("Resume with id {resumeId} deleted.", resumeId);
            }
            catch (BaseException e)
            {
                _logger.LogWarning("Failed to delete resume {resumeId}. With exception: {message}", resumeId, e.Message);
                throw;
            }
        }
    }
}
