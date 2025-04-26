using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Application.Infra.Context;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<Activity> Activities { get; init; }
    public DbSet<Customer> Customers { get; init; }
    public DbSet<Project> Projects { get; init; }
    public DbSet<TeamMember> TeamMembers { get; init; }
    public DbSet<User> Users { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.ConfigureWarnings(warnings =>
        {
            warnings.Ignore(CoreEventId.NavigationBaseIncludeIgnored);
        });

        new DatabaseContextFactory().CreateDbContext();
    }

    public static DatabaseContext CreateDbContext() =>
        new DatabaseContextFactory().CreateDbContext();
}
