namespace Domain.Entities
{
    public class Candidate
    {
        public Guid UserId { get; set; }
        public Guid? ResumeId { get; set; }
        #region references
        public Resume? Resume { get; private set; }
        #endregion
    }
}
