using Domain.Enums;
using HireHive.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HireHive.Infrastructure.Data
{
    public static class DatabaseExtensions
    {
        public static async Task AddInitializerAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var initialiser = scope.ServiceProvider.GetRequiredService<Initialiser>();

            await initialiser.InitialiseDatabaseAsync();
        }

        public static async Task AddSeederAsync(this DbContextOptionsBuilder builder, IServiceProvider serviceProvider)
        {
            var initialiser = serviceProvider.GetRequiredService<Initialiser>();

            await initialiser.TrySeedDatabaseAsync();
        }
    }

    public class Initialiser
    {
        protected readonly ILogger<Initialiser> _logger;
        protected readonly AppDbContext _context;
        protected readonly UserManager<User> _userManager;
        protected readonly RoleManager<IdentityRole<Guid>> _roleManager;
        public Initialiser(
            ILogger<Initialiser> logger,
            AppDbContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitialiseDatabaseAsync()
        {
            try
            {
                await _context.Database.MigrateAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured while initialising the database");
                throw;
            }
        }

        public async Task TrySeedDatabaseAsync()
        {
            try
            {
                await SeedDatabaseAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured while seeding the database");
                throw;
            }
        }

        public async Task SeedDatabaseAsync()
        {
            // seed roles
            foreach (var role in Enum.GetNames(typeof(Roles)))
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }

            // seed admin
            string adminEmail = "t8210030@aueb.gr";
            string adminUserName = "admin";

            if (await _userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new User(adminEmail, "Kyriaki", "Darivaki", EmploymentStatus.Intern);

                await _userManager.CreateAsync(admin, "Password1!@#");
                await _userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
            }
            else
            {
                _logger.LogInformation($"Admin {adminUserName} already exists");
            }

            // seed candidate
            string userEmail = "user123@gmail.com";
            string userUserName = "user123";

            if (await _userManager.FindByEmailAsync(userEmail) == null)
            {
                var user = new User(userEmail, "John", "Doe", EmploymentStatus.Student);

                await _userManager.CreateAsync(user, "Hello1!@#");
                await _userManager.AddToRoleAsync(user, Roles.Candidate.ToString());
            }
            else
            {
                _logger.LogInformation($"User {userUserName} already exists");
            }
        }
    }
}
