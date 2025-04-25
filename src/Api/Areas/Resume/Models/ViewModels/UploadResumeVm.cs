namespace HireHive.Api.Areas.Resume.Models.ViewModels
{
    class UploadResumeVm
    {
        public string? FileName { get; set; }
        public long FileSize { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
