using System.Diagnostics.Contracts;
using System;
using Wcs.Plc.Database;
using Wcs.Plc.DB.Sqlite;

namespace Wcs.Plc.Test
{
  public class Plc : Wcs.Plc.Plc
  {
    private DbContext _db;

    public override DbContext ResolveDbContext()
    {
      Console.WriteLine("wtf");

      if (_db == null) {
        var db = new SqliteDbContext();
        db.UseInMemory();

        var migrator = new Migrator();
        migrator.UseDbContext(db).Migrate();

        _db = db;
      }

      return _db;
    }

    protected override void UseEventLogger()
    {
      var db = ResolveDbContext();
      var logger = new EventLogger(db);

      logger.LogInterval = 0;
      logger.Start();

      Container.Event.Use(logger);
    }
  }
}
