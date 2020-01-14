using Microsoft.EntityFrameworkCore;

namespace DBCore.Sqlite.Test
{
  public class Database : DBCore.DbContext
  {
    public DbSet<User> Users { get; set; }

    private SqliteBuilder _builder;

    public Database()
    {
      _builder = new SqliteBuilder();

      _builder.UseInMemory(this);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      _builder.OnConfiguring(options);
    }
  }
}
