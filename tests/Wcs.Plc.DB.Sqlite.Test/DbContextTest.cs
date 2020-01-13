using System;
using System.Linq;
using NUnit.Framework;
using Wcs.Plc.Entities;

namespace Wcs.Plc.DB.Sqlite.Test
{
  [TestFixture]
  public class DbContextTest
  {
    private SqliteDbContext GetInitializedDB()
    {
      var db = new SqliteDbContext();
      var migrator = new Migrator();

      db.UseInMemory();
      migrator.UseDbContext(db).Migrate();

      return db;
    }

    [Test]
    public void TestMigrator()
    {
      var db = GetInitializedDB();

      Assert.IsTrue(db.HasTable("event_logs"));
    }

    [Test]
    public void TestEventLogCURD()
    {
      var db = GetInitializedDB();
      var eventLog = new EventLog {
        Key = "key",
        Payload = "payload",
        HandlerCount = 0,
      };

      db.EventLogs.Add(eventLog);
      db.SaveChanges();

      var flag = db.EventLogs.Any(item => item.Key == "key");

      Assert.IsTrue(flag);

      var data = db.EventLogs.Where(item => item.Key == "key").First();

      data.Key = "updated";
      db.SaveChanges();
      flag = db.EventLogs.Any(item => item.Key == "updated");
      Assert.IsTrue(flag);

      db.EventLogs.Remove(eventLog);
      db.SaveChanges();

      flag = db.EventLogs.Any(item => item.Key == "key");
      Assert.IsFalse(flag);
    }

  }
}
