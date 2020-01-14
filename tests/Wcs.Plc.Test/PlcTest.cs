using System.Linq;
using NUnit.Framework;

namespace Wcs.Plc.Test
{
  [TestFixture]
  public class PlcTest
  {
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
