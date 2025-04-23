namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public string UserName { get; set; }
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
