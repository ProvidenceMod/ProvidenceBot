using ProvidenceDAL.Database.Models.items;
using Microsoft.EntityFrameworkCore;

namespace ProvidenceDAL.Database
{
  public class SuggestContext : DbContext
  {
    public SuggestContext(DbContextOptions<SuggestContext> options) : base(options) { }
    public DbSet<Item> Items { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //  var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "csharpi.db" };
    //  var connectionString = connectionStringBuilder.ToString();
    //  var connection = new SqliteConnection(connectionString);
    //  optionsBuilder.UseSqlite(connection);
    //}
  }
}