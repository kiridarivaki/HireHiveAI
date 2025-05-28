using HireHive.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HireHive.Infrastructure.Data
{
    public class HireHiveDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DbSet<Resume> Resume { get; set; }
        public HireHiveDbContext(DbContextOptions<HireHiveDbContext> options) : base(options)
        {
        }
    }
}
