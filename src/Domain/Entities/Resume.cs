namespace HireHive.Domain.Entities
{
    public class Resume : BaseEntity<Guid>
    {
        public string? FileName { get; set; }
        public string? BlobName { get; set; }
        public string? ContentType { get; set; }
        public long FileSize { get; set; }
        public Guid UserId { get; set; }
        public Resume(string fileName, string blobName, string contentType, long fileSize, Guid userId) : base()
        {
            FileName = fileName;
            BlobName = blobName;
            ContentType = contentType;
            FileSize = fileSize;
            UserId = userId;
        }
        public void Update(string? fileName, string? blobName, string? contentType, long? fileSize)
        {
            base.Update();

            FileName = fileName ?? FileName;
            BlobName = blobName ?? BlobName;
            ContentType = contentType ?? ContentType;
            FileSize = fileSize ?? FileSize;
        }
    }
}
