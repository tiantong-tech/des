namespace Wcs.Plc
{
  public interface IEventArgs
  {
    string Key { get; }

    string Payload { get; }

    int HandlerCount { get; }
  }
}
