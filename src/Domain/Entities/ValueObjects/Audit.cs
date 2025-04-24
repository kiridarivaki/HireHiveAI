using Microsoft.EntityFrameworkCore;

namespace HireHive.Domain.Entities.ValueObjects
{
    [Owned]
    public class Audit
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Audit()
        {
            var now = DateTime.UtcNow;
            CreatedAt = now;
            UpdatedAt = now;
        }

        public Audit(DateTime createdAt)
        {
            CreatedAt = createdAt;
            UpdatedAt = DateTime.UtcNow;
        }
    }

}
