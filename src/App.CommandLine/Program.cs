using System;
using Wcs.Plc;

namespace App.CommandLine
{
  class Program
  {
    static void Main()
    {
      var plc = new Plc();

      plc.State("hb").Word("D1").Heartbeat(100).Collect(100);
      plc.State("scanner").Words("D2").Collect(100);

      plc.Word("hb").Watch(value => value > 0).Event("event");
      plc.Words("scanner").Watch(value => value != null).Event("scanning");

      plc.Word("hb").On("event", value => Console.WriteLine(value));
      plc.Words("scanner").On("scanning", value => {});

      plc.Words("scanner").Set("ojbk");

      plc.Run();
    }
  }
}
