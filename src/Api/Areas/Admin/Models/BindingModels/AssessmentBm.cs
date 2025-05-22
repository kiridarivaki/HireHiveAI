using HireHive.Domain.Enums;

namespace HireHive.Api.Areas.Admin.Models.BindingModels
{
    public class AssessmentBm
    {
        public string JobDescription { get; set; } = null!;
        public JobType JobTypes { get; set; }
        public List<int> CriteriaWeights { get; set; } = null!;
        public int? Cursor { get; set; }
    }
}
