namespace HireHive.Api.Areas.Resume.Models
{
    public class UploadResumeBm
    {
        public IFormFile File { get; set; }
        public Guid UserId { get; set; }
    }
}
