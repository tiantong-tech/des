using Microsoft.EntityFrameworkCore;

namespace DBCore.Sqlite.Test
{
  public class Database : DBCore.Sqlite.SqliteContext
  {
    public DbSet<User> Users { get; set; }

    public Database()
    {
      UseInMemory();
    }

  }
}
