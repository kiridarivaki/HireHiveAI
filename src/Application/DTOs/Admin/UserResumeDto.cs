namespace HireHive.Application.DTOs.Admin
{
    public class UserResumeDto
    {
        public Guid UserId { get; set; }
        public string? ResumeText { get; set; } = string.Empty;
        public int? MatchPercentage { get; set; }
    }

}
