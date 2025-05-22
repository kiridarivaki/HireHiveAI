using Domain.Enums;
using HireHive.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace HireHive.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public EmploymentStatus EmploymentStatus { get; set; }
        public List<JobType>? JobTypes { get; set; }

        #region references
        public Resume? Resume { get; private set; }
        #endregion

        public User(string email, string firstName, string lastName, EmploymentStatus employmentStatus, List<JobType>? jobTypes)
        {
            Email = email;
            UserName = email;
            FirstName = firstName;
            LastName = lastName;
            EmploymentStatus = employmentStatus;
            JobTypes = jobTypes;
        }

        public void UpdateUser(string? firstName, string? lastName)
        {
            FirstName = firstName ?? FirstName;
            LastName = lastName ?? LastName;
        }
        public void UpdateEmploymentStatus(EmploymentStatus employmentStatus)
        {
            EmploymentStatus = employmentStatus;
        }
        public void UpdateJobTypes(List<JobType>? jobTypes)
        {
            JobTypes = jobTypes;
        }
    }
}
