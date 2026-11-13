using HireHive.Application.DTOs.Admin;
using HireHive.Application.Interfaces;
using HireHive.Domain.Exceptions;
using HireHive.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace HireHive.Infrastructure.Services;

public class AdminService(
    IMapper mapper,
    ILogger<AdminService> logger,
    IUserRepository userRepository,
    IResumeRepository resumeRepository,
    IAssessmentService assessmentService) : IAdminService
{
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<AdminService> _logger = logger;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IResumeRepository _resumeRepository = resumeRepository;
    private readonly IAssessmentService _assessmentService = assessmentService;

    public async Task<List<AssessmentResultDto>>? AssessBatch(AssessmentParamsDto assessmentDto)
    {
        try
        {
            var usersToAssess = await GetResumeBatch(assessmentDto);

            if (usersToAssess == null || !usersToAssess.Any())
            {
                return [];
            }

            var assessmentResults = await _assessmentService.AssessUsers(usersToAssess, assessmentDto);

            var userIds = assessmentResults!.Select(result => result.UserId).ToList();

            var userData = await _userRepository.GetByIds(userIds);

            for (int i = 0; i < userData.Count; i++)
            {
                _mapper.Map(userData[i], assessmentResults[i]);
            }

            _logger.LogInformation("Resume batch assessed successfully.");

            return assessmentResults;
        }
        catch (BaseException e)
        {
            _logger.LogWarning("Assessment of resume batch failed. With exception: {message}", e.Message);
            throw;
        }
    }

    public async Task<List<UserResumeDto>> GetResumeBatch(AssessmentParamsDto assessmentDto)
    {
        try
        {
            int usersAssessed = assessmentDto.Cursor ?? 0;
            int usersToAssess = await _userRepository.CountFiltered(assessmentDto.JobType);

            var resumeBatch = new List<UserResumeDto>();
            var totalTokens = _assessmentService.CountTokens(assessmentDto.JobDescription);
            int assessmentStart = usersAssessed;

            while (assessmentStart < usersToAssess)
            {
                var userWithResume = (await _resumeRepository
                    .GetResumesToAssessAsync(assessmentDto.JobType, assessmentStart, 1))
                    .FirstOrDefault();

                if (userWithResume == null)
                    break;

                var resumeTokens = _assessmentService.CountTokens(userWithResume.Text);

                if (_assessmentService.isTokenLimitReached(totalTokens + resumeTokens))
                    break;

                resumeBatch.Add(new UserResumeDto
                {
                    UserId = userWithResume.UserId,
                    ResumeText = userWithResume.Text
                });

                totalTokens += resumeTokens;
                assessmentStart++;
            }
            _logger.LogInformation("Batch created successfully.");

            return resumeBatch;
        }
        catch (BaseException e)
        {
            _logger.LogWarning("Batch failed to create. With exception: {message}", e.Message);
            throw;
        }
    }

    public List<SortResultDto> SortResults(SortParamsDto sortDataModel)
    {
        try
        {
            if (sortDataModel.AssessmentData == null || string.IsNullOrEmpty(sortDataModel.OrderByField))
                return [];

            var property = typeof(AssessmentResultDto).GetProperty(sortDataModel.OrderByField);
            if (property == null)
                return [];

            var sortedData = sortDataModel.SortOrder?.ToLower() == "asc"
                ? sortDataModel.AssessmentData.OrderBy(x => property.GetValue(x, null)).ToList()
                : [.. sortDataModel.AssessmentData.OrderByDescending(x => property.GetValue(x, null))];

            _logger.LogInformation("Results sorted successfully.");

            return _mapper.Map<List<SortResultDto>>(sortedData);
        }
        catch (BaseException e)
        {
            _logger.LogWarning("Results failed to sort. With exception: {message}", e.Message);
            throw;
        }
    }
}
