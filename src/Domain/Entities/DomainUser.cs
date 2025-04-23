namespace Domain.Entities
{
    public class DomainUser : BaseEntity
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}
