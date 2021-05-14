using csharpi.Database.Models.items;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace csharpi.Database
{
  public class CsharpiContext : DbContext
  {
    public DbSet<Item> Items { get; set; }

     public CsharpiContext(DbContextOptions<CsharpiContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "csharpi.db" };
      var connectionString = connectionStringBuilder.ToString();
      var connection = new SqliteConnection(connectionString);
      optionsBuilder.UseSqlite(connection);
    }
  }
}