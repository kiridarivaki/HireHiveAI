using HireHive.Domain.Enums;

namespace HireHive.Api.Areas.Admin.Models.BindingModels
{
    public class AssessmentParamsBm
    {
        public string JobDescription { get; set; } = null!;
        public JobType JobType { get; set; }
        public List<string> Criteria { get; set; } = null!;
        public List<int> CriteriaWeights { get; set; } = null!;
        public int? Cursor { get; set; }
    }
}
