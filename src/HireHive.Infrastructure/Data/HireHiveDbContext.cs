using HireHive.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HireHive.Infrastructure.Data;

public class HireHiveDbContext(DbContextOptions<HireHiveDbContext> options) : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Resume> Resume { get; set; }
    public DbSet<Job> Job { get; set; }
    public DbSet<JobAssessment> JobAssessment { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Job>(job =>
        {
            job.Property(j => j.Title).HasMaxLength(200);
            job.OwnsMany(j => j.Criteria, criteria => criteria.ToJson());
            job.HasOne(j => j.Owner)
                .WithMany()
                .HasForeignKey(j => j.OwnerId);
            job.HasIndex(j => new { j.OwnerId, j.Status });
        });

        builder.Entity<JobAssessment>(assessment =>
        {
            assessment.HasOne(a => a.Job)
                .WithMany(j => j.Assessments)
                .HasForeignKey(a => a.JobId);
            assessment.HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId);
            assessment.HasIndex(a => new { a.JobId, a.UserId }).IsUnique();
        });
    }
}
