using Domain.Enums;

namespace Domain.Entities
{
    public class Candidate
    {
        public Guid UserId { get; set; }
        public Guid? ResumeId { get; set; }
        public EmploymentStatus EmploymentStatus { get; set; }

        #region references
        public Resume? Resume { get; private set; }
        #endregion
        public Candidate()
        {

        }
        public Candidate(Guid userId, EmploymentStatus employmentStatus, Guid? resumeId = null)
        {
            UserId = userId;
            EmploymentStatus = employmentStatus;
            ResumeId = resumeId;
        }
        public void UpdateEmploymentStatus(EmploymentStatus newStatus)
        {
            EmploymentStatus = newStatus;
        }
    }
}
