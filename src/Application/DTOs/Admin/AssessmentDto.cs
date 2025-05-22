using HireHive.Domain.Enums;

namespace HireHive.Application.DTOs.Admin
{
    public class AssessmentDto
    {
        public string JobDescription { get; set; } = null!;
        public JobType JobType { get; set; }
        public List<int> CriteriaWeights { get; set; } = null!;
        public int? Cursor { get; set; }
    }
}
