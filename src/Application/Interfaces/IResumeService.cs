using HireHive.Application.DTOs.Resume;

namespace HireHive.Application.Interfaces
{
    public interface IResumeService
    {
        Task<ResumeDto> GetByUserId(Guid resumeId);
        Task<UploadResumeDto> Upload(UploadResumeDto uploadDto);
        Task Update(Guid resumeId, UpdateResumeDto updateResumeDto);
        Task Delete(Guid resumeId);
    }
}
