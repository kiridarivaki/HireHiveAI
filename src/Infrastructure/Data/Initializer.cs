using Domain.Enums;
using HireHive.Domain.Entities;
using HireHive.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Reflection;

namespace HireHive.Infrastructure.Data
{
    public static class Initializer
    {
        public static async Task Seed(AppDbContext context, UserManager<User> userManager, IConfiguration configuration)
        {
            if (await context.Roles.AnyAsync()) return;

            var connectionString = configuration["HireHivePostgresConnectionString"];
            var dataSource = NpgsqlDataSource.Create(connectionString!);
            var postgresConnection = await dataSource.OpenConnectionAsync();

            await using var transaction = await postgresConnection.BeginTransactionAsync();

            try
            {
                var script = await LoadScript("InsertRoles");

                await using var command = new NpgsqlCommand(script, postgresConnection, transaction);

                //todo : fix using roles in sql script
                string[] roles = Enum.GetNames(typeof(Roles));
                command.Parameters.AddWithValue("roles", roles);
                await command.ExecuteNonQueryAsync();

                await transaction.CommitAsync();

                await SeedAdmin(userManager, context);
            }
            catch (NpgsqlException)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private static async Task<string> LoadScript(string scriptName)
        {
            const string assemblyName = "HireHive.Infrastructure";
            var assembly = Assembly.Load(assemblyName);
            var resourceName = assemblyName + $".Data.Scripts.{scriptName}.sql";

            await using var stream = assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream!);

            return await reader.ReadToEndAsync();
        }

        public static async Task SeedAdmin(UserManager<User> userManager, AppDbContext context)
        {
            // seed admin
            string adminEmail = "t8210030@aueb.gr";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new User(adminEmail, "Kyriaki", "Darivaki", EmploymentStatus.Intern, null);

                var result = await userManager.CreateAsync(admin, "Password1!@#");
                if (!result.Succeeded)
                    throw new InvalidOperationException($"Failed to create default admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");

                await userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
            }
            await context.SaveChangesAsync();

            // seed candidate
            string userEmail = "user123@gmail.com";

            if (await userManager.FindByEmailAsync(userEmail) == null)
            {
                var user = new User(userEmail, "John", "Doe", EmploymentStatus.Student, [JobType.DataScientist]);

                var result = await userManager.CreateAsync(user, "Hello1!@#");
                if (!result.Succeeded)
                    throw new InvalidOperationException($"Failed to create default user: {string.Join(", ", result.Errors.Select(e => e.Description))}");

                await userManager.AddToRoleAsync(user, Roles.Candidate.ToString());
            }
            await context.SaveChangesAsync();
        }
    }
}
