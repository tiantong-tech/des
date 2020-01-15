using System.Linq;
using NUnit.Framework;
using Wcs.Plc.Entities;

namespace Wcs.Plc.Test
{
  [TestFixture]
  public class PlcTest
  {
    [Test]
    public void TestResolvePlcConnection()
    {
      var plc = new Plc();
      var db = plc.ResolveDbContext();

      plc.Name("test").Model("melsec").Host("localhost").Port("1234");
      plc.ResolvePlcConnection();

      var id = plc.Connection.Id;

      Assert.IsTrue(db.PlcConnections.Any(item => item.Id == id));

      plc.Connection = new PlcConnection();
      plc.Id(id);
      plc.ResolvePlcConnection();

      Assert.AreEqual(plc.Connection.Port, "1234");
      Assert.AreEqual(plc.Connection.Name, "test");
      Assert.AreEqual(plc.Connection.Model, "melsec");
      Assert.AreEqual(plc.Connection.Host, "localhost");
    }

    [Test]
    public void TestPlcCollectRunStop()
    {
      var plc = new Plc();

      plc.State("bit data").Bit("D1").Collect(0);
      plc.Watch("bit data", "==", true).Event("event");
      plc.Bit("bit data").Set(true);
      plc.On<bool>("event", val => {
        _ = plc.StopAsync();
      });

      plc.Run();
    }

    [Test]
    public void TestPlcHeartbeat()
    {
      var plc = new Plc();

      plc.State("hb").Word("D1").Heartbeat(0).Collect(0);
      plc.Watch<int>("hb", value => value > 10).Event("stop");
      plc.On<int>("stop", val => {
        _ = plc.StopAsync();
      });
      plc.Run();
    }

    [Test]
    public void TestEventLogger()
    {
      var plc = new Plc();
      var db = plc.ResolveDbContext();

      plc.State("hb").Word("D1").Heartbeat(0).Collect(0);
      plc.Watch<int>("hb", value => value != 0).Event("event");
      plc.On<int>("event", val => {
        _ = plc.StopAsync();
      });

      plc.Run();

      var count = db.EventLogs.Where(log => log.Key == "event").Count();

      Assert.AreEqual(1, count);
    }
  }
}
