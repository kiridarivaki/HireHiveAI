namespace HireHive.Application.DTOs.Resume
{
    public class ResumeDto
    {
        public Guid Id { get; set; }
        public string? FileName { get; set; }
        public string? BlobName { get; set; }
        public string? ContentType { get; set; }
        public long FileSize { get; set; }
        public Guid UserId { get; set; }
    }
}
