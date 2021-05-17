using ProvidenceBotDAL.Models.Item;
using Microsoft.EntityFrameworkCore;

namespace ProvidenceBotDAL
{
  public class SuggestContext : DbContext
  {
    public SuggestContext(DbContextOptions<SuggestContext> options) : base(options) { }
    public DbSet<Suggestion> Suggestions { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //  var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "csharpi.db" };
    //  var connectionString = connectionStringBuilder.ToString();
    //  var connection = new SqliteConnection(connectionString);
    //  optionsBuilder.UseSqlite(connection);
    //}
  }
}