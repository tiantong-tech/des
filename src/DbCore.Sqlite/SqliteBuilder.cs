using Microsoft.EntityFrameworkCore;

namespace DBCore.Sqlite
{
  public class SqliteBuilder
  {
    private bool _isInMemory = false;

    private string _dbFile = "./sqlite.db";

    public SqliteBuilder UseInMemory(DbContext db)
    {
      _isInMemory = true;
      db.Database.OpenConnection();

      return this;
    }

    public void UseDbFile(string filePath)
    {
      _dbFile = filePath;
    }

    public void OnConfiguring(DbContextOptionsBuilder options)
    {
      if (_isInMemory) {
        options.UseSqlite("DataSource=:memory:");
      } else {
        options.UseSqlite($"DataSource=./{_dbFile}");
      }
    }
  }
}
