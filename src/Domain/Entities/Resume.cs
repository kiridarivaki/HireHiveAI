namespace Domain.Entities
{
    public class Resume : BaseEntity
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
