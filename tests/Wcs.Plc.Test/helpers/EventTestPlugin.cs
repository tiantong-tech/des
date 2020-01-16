using System.Collections.Generic;

namespace Wcs.Plc.Test
{
  using Logs = List<IEventArgs>;

  public class EventTestPlugin : IEventPlugin
  {
    public Logs Logs = new Logs();

    public void Install(IEvent event_)
    {
      event_.All(args => Logs.Add(args));
    }
  }
}
