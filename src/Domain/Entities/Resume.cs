namespace Domain.Entities
{
    public class Resume : BaseEntity
    {
        // todo: add blob name migration
        public string? FileName { get; set; }
        public string? BlobName { get; set; }
        public string? ContentType { get; set; }
        public long FileSize { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public Guid UserId { get; set; }
    }
}
