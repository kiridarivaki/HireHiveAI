namespace Api.Areas.Resume.Models
{
    public class UploadResumeBindingModel
    {
        public IFormFile File { get; set; }
        public Guid UserId { get; set; }
    }
}
