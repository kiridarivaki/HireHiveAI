using HireHive.Application.DTOs.Admin;

namespace HireHive.Application.Interfaces
{
    public interface IAdminService
    {
        Task<List<AssessmentResultDto>> AssessBatch(AssessmentParamsDto assessmentDto);
        List<UserResumeDto> GetResumeBatch(AssessmentParamsDto assessmentDto);
    }
}
