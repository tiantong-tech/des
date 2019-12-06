using System;

namespace Wcs.Plc
{
  public interface IStateWatcher<T>
  {
    void Cancel();

    void Event();

    void EventVoid();

    void Event<R>(string key, R payload);

    void Event(string key, Func<T, T> handler);

    void Event<R>(string key, Func<T, R> handler);
  }
}
