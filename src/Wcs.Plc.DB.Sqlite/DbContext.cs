using System.IO;
using Wcs.Plc.Entities;
using Microsoft.EntityFrameworkCore;

namespace Wcs.Plc.DB.Sqlite
{
  public class SqliteDbContext : DBCore.Sqlite.SqliteContext
  {
    public DbSet<EventLog> EventLogs { get; set; }

    public SqliteDbContext()
    {
      if (!Directory.Exists("./Data/Sqlite")) {
        Directory.CreateDirectory("./Data/Sqlite");
      }

      UseDbFile("./DataSource/sqlite.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }

  }
}
