using Microsoft.Data.Sqlite;

namespace KobzaReferee.Persistence.Sqlite;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var connectionStringBuilder = new SqliteConnectionStringBuilder
        {
            DataSource = "your-database.db",

        };

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlite(connectionStringBuilder.ConnectionString);

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
