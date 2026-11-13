using HireHive.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HireHive.Infrastructure.Data;

public class HireHiveDbContext(DbContextOptions<HireHiveDbContext> options) : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Resume> Resume { get; set; }
}
