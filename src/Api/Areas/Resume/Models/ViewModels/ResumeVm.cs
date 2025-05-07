namespace HireHive.Api.Areas.Resume.Models.ViewModels
{
    public class ResumeVm
    {
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
        public long FileSize { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
