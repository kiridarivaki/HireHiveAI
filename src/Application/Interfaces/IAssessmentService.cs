using HireHive.Application.DTOs.Admin;

namespace HireHive.Application.Interfaces
{
    public interface IAssessmentService
    {
        public int CountTokens(string? text);
        public bool isTokenLimitReached(int tokenCount);
        Task<List<AssessmentResultDto>> AssessUsers(List<UserResumeDto> usersToAssess, AssessmentParamsDto assessmentDto);
    }
}
