using CleanArc.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CleanArc.Tests.Setup.Setups;

public abstract class TestApplicationDbContext
{
    protected ApplicationDbContext UnitTestDbContext { get; }

    protected TestApplicationDbContext()
    {
        var connectionStringBuilder =
            new SqliteConnectionStringBuilder { DataSource = ":memory:" };
        var connection = new SqliteConnection(connectionStringBuilder.ToString());

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;

        UnitTestDbContext = new ApplicationDbContext(options);
    }
}