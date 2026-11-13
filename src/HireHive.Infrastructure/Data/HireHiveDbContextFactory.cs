using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HireHive.Infrastructure.Data;

public class HireHiveDbContextFactory : IDesignTimeDbContextFactory<HireHiveDbContext>
{
    public HireHiveDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("HIREHIVE_DESIGN_CONNECTION")
            ?? "Host=localhost;Database=hirehive;Username=postgres;Password=postgres";

        var options = new DbContextOptionsBuilder<HireHiveDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new HireHiveDbContext(options);
    }
}
