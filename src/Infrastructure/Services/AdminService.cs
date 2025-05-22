using HireHive.Application.DTOs.Admin;
using HireHive.Application.Interfaces;
using HireHive.Domain.Interfaces;
using HireHive.Infrastructure.Services.AI;
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
        public AdminService(
            IMapper mapper,
            ILogger<AdminService> logger,
            IUserRepository userRepository,
            IResumeRepository resumeRepository,
            TokenCountingService tokenCountingService,
            AiAssessmentService aiAssessmentService)
        {
            _mapper = mapper;
            _logger = logger;
            _userRepository = userRepository;
            _resumeRepository = resumeRepository;
            _tokenCountingService = tokenCountingService;
            _aiAssessmentService = aiAssessmentService;
        }
        public List<UserResumeDto> AssessBatch(AssessmentDto assessmentDto)
        {
            var usersToAssess = GetResumeBatch(assessmentDto);
            if (usersToAssess != null)
                var matchPercentages = _aiAssessmentService.AssessUsers(usersToAssess, assessmentDto);
        }
        public List<UserResumeDto> GetResumeBatch(AssessmentDto assessmentDto)
        {
            int usersAssessed = assessmentDto.Cursor ?? 0;
            int usersToAssess = _userRepository.CountFiltered(assessmentDto.JobType);

            var resumeBatch = new List<UserResumeDto>();
            var totalTokens = _tokenCountingService.CountTokens(assessmentDto.JobDescription);
            int cursor = usersAssessed;

            while (cursor < usersToAssess)
            {
                var userWithResume = _resumeRepository
                    .GetResumesToAssess(assessmentDto.JobType, cursor, 1)
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
                cursor++;
            }

            return resumeBatch;
        }
    }
}
