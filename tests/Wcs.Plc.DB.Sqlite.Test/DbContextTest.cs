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
    public void TestEntity()
    {
      var db = GetInitializedDB();
      var eventLog = new EventLog {
        Key = "key",
        Payload = "payload",
        HandlerCount = 0,
      };
      var plcConnection = new PlcConnection {
        Model = "melsec-q",
        Name = "lift",
      };
      var plcConnectionLog = new PlcConnectionLog {
        PlcId = 1,
        Operation = "test",
      };
      var plcStateLog = new PlcStateLog {
        PlcId = 1,
        Operation = "test",
        Name = "test",
        Key = "D1001",
        Value = "100"
      };

      db.EventLogs.Add(eventLog);
      db.PlcConnections.Add(plcConnection);
      db.PlcConnectionLogs.Add(plcConnectionLog);
      db.PlcStateLogs.Add(plcStateLog);
      db.SaveChanges();

      Assert.AreEqual(1, eventLog.Id);
      Assert.AreEqual(1, db.EventLogs.Where(item => item.Key == "key").Count());
      Assert.AreEqual(1, db.PlcConnections.Where(item => item.Name == "lift").Count());
      Assert.AreEqual(1, db.PlcConnectionLogs.Where(item => item.Operation == "test").Count());
      Assert.AreEqual(1, db.PlcStateLogs.Where(item => item.Operation == "test").Count());

      var data = new PlcConnection {
        Name = "updated",
        Model = "updated"
      };

      var conn = db.PlcConnections.SingleOrDefault(item => item.Id == 1);

      data.Id = conn.Id;
      conn = data;

      db.SaveChanges();

      conn = db.PlcConnections.SingleOrDefault(item => item.Id == 1);

      Assert.AreEqual("updated", conn.Model);
    }

  }
}
