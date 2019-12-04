namespace Wcs.Plc
{
  public abstract class EventPlugin : IEventPlugin
  {
    public abstract void Install(IEvent event_);
  }
}
