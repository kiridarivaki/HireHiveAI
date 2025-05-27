namespace HireHive.Application.DTOs.Admin
{
    public class SortDataDto
    {
        public List<AssessmentResultDto>? AssessmentData { get; set; }
        public string? SortOrder { get; set; }
        public string? OrderByField { get; set; }
    }
}
