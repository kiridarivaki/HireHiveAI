namespace HireHive.Application.DTOs.Admin
{
    public class SortParamsDto
    {
        public List<AssessmentResultDto>? AssessmentData { get; set; }
        public string? SortOrder { get; set; }
        public string? OrderByField { get; set; }
    }
}
