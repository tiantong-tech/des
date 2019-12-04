namespace Wcs.Plc
{
  public class EventArgs : IEventArgs
  {
    public string Key { get; set; }

    public string Payload { get; set; }

    public int HandlerCount { get; set; }
  }
}
