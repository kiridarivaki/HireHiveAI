namespace HireHive.Application.Interfaces
{
    public interface IResumeProcessingJob
    {
        Task ProcessResumeText(Guid userId, byte[] fileBytes);
    }
}
