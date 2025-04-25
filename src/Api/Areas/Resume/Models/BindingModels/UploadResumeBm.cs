namespace HireHive.Api.Areas.Resume.Models.BindingModels
{
    public class UploadResumeBm
    {
        public IFormFile File { get; set; } = null!;
        public Guid UserId { get; set; }
    }
}
