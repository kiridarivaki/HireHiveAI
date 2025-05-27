using HireHive.Application.DTOs.Admin;

namespace HireHive.Application.Interfaces
{
    public interface IAiService
    {
        public int CountTokens(string? text);
        public bool isTokenLimitReached(int tokenCount);
        Dictionary<Guid, int> AssessUsers(List<UserResumeDto> usersToAssess, AssessmentParamsDto assessmentDto);
    }
}
