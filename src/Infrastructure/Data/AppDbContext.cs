using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public DbSet<Candidate> Candidate { get; set; }
        public DbSet<Resume> Resume { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Candidate>()
                .HasKey(c => c.UserId);
            modelBuilder.Entity<Candidate>()
                .HasOne<AppUser>()
                .WithOne()
                .HasForeignKey<Candidate>(c => c.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
