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
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }

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

        public void UpdateUser(string? firstName, string? lastName, EmploymentStatus? employmentStatus, List<JobType>? jobTypes)
        {
            FirstName = firstName ?? FirstName;
            LastName = lastName ?? LastName;
            EmploymentStatus = employmentStatus ?? EmploymentStatus;
            JobTypes = jobTypes ?? JobTypes;
        }
    }
}
