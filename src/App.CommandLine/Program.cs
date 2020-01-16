using System;
using Wcs.Plc;
using Wcs.Plc.DB.Sqlite;
using System.Threading.Tasks;
using Wcs.Plc.Entities;

namespace App.CommandLine
{
  class Program
  {
    static void Main(string[] argv)
    {
      var plc = new Plc();
      var mg = new Migrator();

      mg.Migrate();
      plc.Name("test").Model("test").Host("localhost").Port("1234");

      plc.State("hb").Word("D100").Heartbeat(100).Collect(100);
      plc.State("test").Bit("M1").Collect(100);
      plc.State("test2").Bit("M2").Collect(100);
      plc.State("test3").Bit("M3").Collect(100);
      plc.State("test4").Bit("M4").Collect(100);

      plc.Watch<int>("hb", value => value > 0).Event("event");
      plc.On<int>("event", value => Console.WriteLine(value));

      plc.Bit("test").Set(true);
      plc.Bit("test2").Set(false);
      plc.Run();
    }
  }
}
