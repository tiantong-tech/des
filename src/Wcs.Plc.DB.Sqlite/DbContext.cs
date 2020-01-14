using System.IO;
using DBCore.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Wcs.Plc.DB.Sqlite
{
  public class SqliteDbContext : Wcs.Plc.Database.DbContext
  {
    private SqliteBuilder _builder;

    public SqliteDbContext()
    {
      if (!Directory.Exists("./DataSource")) {
        Directory.CreateDirectory("./DataSource");
      }

      _builder = new SqliteBuilder();

      _builder.UseDbFile("./DataSource/sqlite.db");
    }

    public void UseInMemory()
    {
      _builder.UseInMemory(this);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      _builder.OnConfiguring(options);
    }

  }
}
