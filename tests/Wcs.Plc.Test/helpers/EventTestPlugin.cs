using System.Collections.Generic;

namespace Wcs.Plc.Test
{
  using Logs = List<IEventArgs>;

  public class EventTestPlugin : EventPlugin
  {
    public Logs Logs = new Logs();

    public override void Install(IEvent event_)
    {
      event_.All(args => Logs.Add(args));
    }
  }
}
