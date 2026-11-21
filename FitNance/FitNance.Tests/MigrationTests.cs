using FitNance.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FitNance.Tests;

public class MigrationTests
{
    [Fact]
    public void SqlServerModelMatchesLatestMigration()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer()
            .Options;

        using var context = new AppDbContext(options);

        Assert.False(context.Database.HasPendingModelChanges(),
            "Add an EF Core migration for the pending SQL Server model changes.");
    }
}
