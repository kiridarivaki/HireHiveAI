using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public AppUser()
        {
            UserName = Email;
        }
    }
}
