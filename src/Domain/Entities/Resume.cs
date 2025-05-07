namespace HireHive.Domain.Entities
{
    public class Resume : BaseEntity<Guid>
    {
        public string? FileName { get; set; }
        public string? BlobName { get; set; }
        public string? ContentType { get; set; }
        public long? FileSize { get; set; }
        public string? Text { get; set; }
        public Guid UserId { get; set; }
        #region references
        public User User { get; set; } = null!;
        #endregion

        public Resume() : base() { }
        public Resume(string fileName, string blobName, string contentType, long fileSize, Guid userId, string? text = null) : base()
        {
            FileName = fileName;
            BlobName = blobName;
            ContentType = contentType;
            FileSize = fileSize;
            UserId = userId;
            Text = text;
        }
        public void Update(string? fileName = null,
            string? blobName = null,
            string? contentType = null,
            long? fileSize = null,
            string? text = null)
        {
            base.Update();

            FileName = fileName ?? FileName;
            BlobName = blobName ?? BlobName;
            ContentType = contentType ?? ContentType;
            FileSize = fileSize ?? FileSize;
            Text = text ?? Text;
        }
    }
}
