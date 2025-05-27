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
        private readonly IUserService _userService;
        public AdminService(
            IMapper mapper,
            ILogger<AdminService> logger,
            IUserRepository userRepository,
            IResumeRepository resumeRepository,
            IAiService aiService,
            IUserService userService)
        {
            _mapper = mapper;
            _logger = logger;
            _userRepository = userRepository;
            _resumeRepository = resumeRepository;
            _aiService = aiService;
            _userService = userService;
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

                return assessmentResult;
            }
            catch (BaseException e)
            {
                _logger.LogWarning("Assessment of resume batch failed. With message: {message}", e.Message);
                throw;
            }

        }
        public List<UserResumeDto> GetResumeBatch(AssessmentParamsDto assessmentDto)
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

            return resumeBatch;
        }

        public async Task<SortDataDto> SortResults(SortDataDto sortDataModel)
        {
            throw new NotImplementedException();
        }
    }
}
