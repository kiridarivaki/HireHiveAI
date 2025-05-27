using HireHive.Application.DTOs.Admin;
using HireHive.Application.Interfaces;
using HireHive.Domain.Exceptions;
using HireHive.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace HireHive.Infrastructure.Services
{
    public class AdminService : IAdminService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AdminService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IResumeRepository _resumeRepository;
        private readonly IAiService _aiService;
        public AdminService(
            IMapper mapper,
            ILogger<AdminService> logger,
            IUserRepository userRepository,
            IResumeRepository resumeRepository,
            IAiService aiService)
        {
            _mapper = mapper;
            _logger = logger;
            _userRepository = userRepository;
            _resumeRepository = resumeRepository;
            _aiService = aiService;
        }
        public async Task<List<AssessmentResultDto>> AssessBatch(AssessmentParamsDto assessmentDto)
        {
            try
            {
                var usersToAssess = GetResumeBatch(assessmentDto);

                if (usersToAssess == null || !usersToAssess.Any())
                {
                    return new List<AssessmentResultDto>();
                }

                var matchPercentages = _aiService.AssessUsers(usersToAssess, assessmentDto);

                var users = await _userRepository.GetByIds(matchPercentages.Keys.ToList());

                var assessmentResult = users.Select(u => new AssessmentResultDto
                {
                    UserId = u.Id,
                    Email = u.Email!,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    EmploymentStatus = u.EmploymentStatus,
                    MatchPercentage = matchPercentages[u.Id]
                }).ToList();

                _logger.LogInformation("Resume batch assessed successfully.");

                return assessmentResult;
            }
            catch (BaseException e)
            {
                _logger.LogWarning("Assessment of resume batch failed. With exception: {message}", e.Message);
                throw;
            }

        }
        public List<UserResumeDto> GetResumeBatch(AssessmentParamsDto assessmentDto)
        {
            try
            {
                int usersAssessed = assessmentDto.Cursor ?? 0;
                int usersToAssess = _userRepository.CountFiltered(assessmentDto.JobType);

                var resumeBatch = new List<UserResumeDto>();
                var totalTokens = _aiService.CountTokens(assessmentDto.JobDescription);
                int assessmentStart = usersAssessed;

                while (assessmentStart < usersToAssess)
                {
                    var userWithResume = _resumeRepository
                        .GetResumesToAssess(assessmentDto.JobType, assessmentStart, 1)
                        .FirstOrDefault();

                    if (userWithResume == null)
                        break;

                    var resumeTokens = _aiService.CountTokens(userWithResume.Text);

                    if (_aiService.isTokenLimitReached(totalTokens + resumeTokens))
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

        public List<SortResultDto> SortResults(SortDataDto sortDataModel)
        {
            try
            {
                if (sortDataModel.AssessmentData == null || string.IsNullOrEmpty(sortDataModel.OrderByField))
                    return new List<SortResultDto>();

                var property = typeof(AssessmentResultDto).GetProperty(sortDataModel.OrderByField);
                if (property == null)
                    return new List<SortResultDto>();

                var sortedData = sortDataModel.SortOrder?.ToLower() == "asc"
                    ? sortDataModel.AssessmentData.OrderBy(x => property.GetValue(x, null)).ToList()
                    : sortDataModel.AssessmentData.OrderByDescending(x => property.GetValue(x, null)).ToList();

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
}
