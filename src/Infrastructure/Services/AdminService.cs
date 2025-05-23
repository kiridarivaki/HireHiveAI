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
        private readonly TokenCountingService _tokenCountingService;
        private readonly AiAssessmentService _aiAssessmentService;
        private readonly IUserService _userService;
        public AdminService(
            IMapper mapper,
            ILogger<AdminService> logger,
            IUserRepository userRepository,
            IResumeRepository resumeRepository,
            TokenCountingService tokenCountingService,
            AiAssessmentService aiAssessmentService,
            IUserService userService)
        {
            _mapper = mapper;
            _logger = logger;
            _userRepository = userRepository;
            _resumeRepository = resumeRepository;
            _tokenCountingService = tokenCountingService;
            _aiAssessmentService = aiAssessmentService;
            _userService = userService;
        }
        public async Task<List<AssessmentResultDto>> AssessBatch(AssessmentParamsDto assessmentDto)
        {
            try
            {
                var usersToAssess = GetResumeBatch(assessmentDto);

                var assessmentResult = _aiAssessmentService.AssessUsers(usersToAssess, assessmentDto);

                var userIds = usersToAssess.Select(u => u.UserId).ToList();

                var userInfo = await _userService.GetByIds(userIds);

                _mapper.Map(userInfo, assessmentResult);

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
            var totalTokens = _tokenCountingService.CountTokens(assessmentDto.JobDescription);
            int assessmentStart = usersAssessed;

            while (assessmentStart < usersToAssess)
            {
                var userWithResume = _resumeRepository
                    .GetResumesToAssess(assessmentDto.JobType, assessmentStart, 1)
                    .FirstOrDefault();

                if (userWithResume == null)
                    break;

                var resumeTokens = _tokenCountingService.CountTokens(userWithResume.Text);

                if (_tokenCountingService.isTokenLimitReached(totalTokens + resumeTokens))
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
    }
}
