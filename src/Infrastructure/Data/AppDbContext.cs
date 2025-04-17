using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Infrastructure.Identity;

namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        DbSet<Resume> Resume { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.UseSerialColumns();
        }
    }
}
