using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data
{
    public static class DatabaseExtensions
    {
        public static async Task AddInitializerAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var initialiser = scope.ServiceProvider.GetRequiredService<AppDbContextInitialiser>();

            await initialiser.InitialiseDatabaseAsync();
        }
        
        public static async Task AddSeederAsync(this DbContextOptionsBuilder builder, IServiceProvider serviceProvider)
        {
            var initialiser = serviceProvider.GetRequiredService<AppDbContextInitialiser>();

            await initialiser.TrySeedDatabaseAsync();
        }
    }

    public class AppDbContextInitialiser
    {
        protected readonly ILogger<AppDbContextInitialiser> _logger;
        protected readonly AppDbContext _context;
        protected readonly UserManager<AppUser> _userManager;
        protected readonly RoleManager<IdentityRole> _roleManager;
        public AppDbContextInitialiser(
            ILogger<AppDbContextInitialiser> logger,
            AppDbContext context, 
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager)
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
            var roles = new[] { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // seed admin
            string adminEmail = "t8210030@aueb.gr";
            string adminUserName = "admin";

            if (await FindUserByEmail(adminEmail) == null)
            {
                AppUser admin = new()
                {
                    Email = adminEmail,
                    NormalizedEmail = adminEmail.ToUpper(),
                    UserName = adminUserName,
                    NormalizedUserName = adminUserName.ToUpper(),
                    FirstName = "Kyiaki",
                    LastName = "Darivaki"
                };

                await _userManager.CreateAsync(admin, "Password1!@#");
                await _userManager.AddToRoleAsync(admin, "Admin");
            }
            else
            {
                _logger.LogWarning($"Admin {adminUserName} already exists");
            }

            // seed users
            string userEmail = "user123@gmail.com";
            string userUserName = "user123";

            if (await FindUserByEmail(userEmail) == null)
            {
                AppUser user = new()
                {
                    Email = userEmail,
                    NormalizedEmail = userEmail.ToUpper(),
                    UserName = userUserName,
                    NormalizedUserName = userUserName.ToUpper(),
                    FirstName = "Vaggelis",
                    LastName = "Daivakis"
                };

                await _userManager.CreateAsync(user, "Hello1!@#");
                await _userManager.AddToRoleAsync(user, "User");
            }
            else
            {
                _logger.LogWarning($"User {userUserName} already exists");
            }
        }
        //todo: seed resume

        public async Task<AppUser?> FindUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
    }
}
