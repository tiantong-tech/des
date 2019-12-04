using System;

namespace Wcs.Plc
{
  public class EventListener : IEventListener
  {
    private Action _cancel;

    public EventListener(Action Cancel)
    {
      _cancel = Cancel;
    }

    public void Cancel()
    {
      _cancel();
    }
  }
}
