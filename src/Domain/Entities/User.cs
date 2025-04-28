using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace HireHive.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public EmploymentStatus EmploymentStatus { get; set; }

        #region references
        public Resume? Resume { get; private set; }
        #endregion

        public User(string email, string firstName, string lastName, EmploymentStatus employmentStatus)
        {
            Email = email;
            UserName = email;
            FirstName = firstName;
            LastName = lastName;
            EmploymentStatus = employmentStatus;
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
    }
}
