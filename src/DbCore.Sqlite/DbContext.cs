using Microsoft.EntityFrameworkCore;

namespace DBCore.Sqlite
{
  public class SqliteContext : DBCore.DbContext
  {
    private bool _isInMemory = false;

    private string _dbFile = "./sqlite.db";

    public SqliteContext UseInMemory()
    {
      _isInMemory = true;
      OpenConnection();

      return this;
    }

    public void UseDbFile(string filePath)
    {
      _dbFile = filePath;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      if (_isInMemory) {
        options.UseSqlite("DataSource=:memory:");
      } else {
        options.UseSqlite($"DataSource=./{_dbFile}");
      }
    }

    public SqliteContext OpenConnection()
    {
      Database.OpenConnection();

      return this;
    }

  }
}
