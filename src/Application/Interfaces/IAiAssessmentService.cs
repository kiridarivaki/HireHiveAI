using HireHive.Application.DTOs.Admin;

namespace HireHive.Application.Interfaces
{
    public interface IAiAssessmentService
    {
        Dictionary<Guid, int> AssessUsers(List<UserResumeDto> usersToAssess, AssessmentParamsDto assessmentDto);
    }
}
