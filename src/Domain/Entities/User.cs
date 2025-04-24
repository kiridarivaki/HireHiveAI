using Microsoft.AspNetCore.Identity;

namespace HireHive.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public User(string email, string firstName, string lastName)
        {
            Email = email;
            UserName = email;
            FirstName = firstName;
            LastName = lastName;
        }
        public void UpdateUser(string? firstName = null, string? lastName = null)
        {
            if (firstName != null)
            {
                FirstName = firstName;

            }

            if (lastName != null)
            {
                LastName = lastName;
            }
        }
    }
}
