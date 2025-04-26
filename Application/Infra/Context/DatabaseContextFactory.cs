using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Application.Infra.Context;

public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext()
    {
        string[] args = [""];
        return CreateDbContext(args);
    }

    public DatabaseContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING")
            ?? throw new Exception("Connection string not found");

        // Postgres
        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        optionsBuilder
            .UseNpgsql(
                connectionString,
                options =>
                {
                    options.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                    options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                }
            )
            .EnableSensitiveDataLogging(false)
            .EnableDetailedErrors(false);

        return new DatabaseContext(optionsBuilder.Options);
    }
}
