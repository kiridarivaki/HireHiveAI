using HireHive.Application.DTOs.Admin;

namespace HireHive.Application.Interfaces
{
    public interface IAdminService
    {
        List<UserResumeDto> GetResumeBatch(AssessmentDto assessmentDto);
        List<UserResumeDto> AssessBatch(AssessmentDto assessmentDto);
    }
}
