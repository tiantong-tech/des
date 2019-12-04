using System;
using System.Threading.Tasks;

namespace Wcs.Plc
{
  public interface IEvent
  {
    IEventListener On(string key, Action handler);

    IEventListener On(string key, Func<Task> handler);

    IEventListener On<T>(string key, Action<T> handler);

    IEventListener On<T>(string key, Func<T, Task> handler);

    IEventListener Once(string key, Action handler);

    IEventListener Once(string key, Func<Task> handler);

    IEventListener Once<T>(string key, Action<T> handler);

    IEventListener Once<T>(string key, Func<T, Task> handler);

    IEventListener All(Action<IEventArgs> handler);

    IEventListener All(Func<IEventArgs, Task> handler);

    Task EmitAsync(string key);

    Task EmitAsync<T>(string key, T payload);

    void Emit(string key);

    void Emit<T>(string key, T payload);
  }
}
