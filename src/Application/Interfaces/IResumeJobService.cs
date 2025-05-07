namespace HireHive.Application.Interfaces
{
    public interface IResumeJobService
    {
        Task ProcessResume(byte[] fileBytes, Guid userId);
    }
}
