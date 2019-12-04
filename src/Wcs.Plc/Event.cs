using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Wcs.Plc
{
  using Handler = Func<string, Task>;
  using Handlers = Dictionary<int, Func<string, Task>>;
  using EventPool = Dictionary<string, Dictionary<int, Func<string, Task>>>;
  using GlobalHandlers = Dictionary<int, Func<IEventArgs, Task>>;

  public class Event : IEvent
  {
    private int _id = 0;

    private EventPool _eventPool = new EventPool();

    private GlobalHandlers _globalHandlers = new GlobalHandlers();

    public Event()
    {

    }

    public Event(EventPlugin plugin)
    {
      Use(plugin);
    }

    public void Use(EventPlugin plugin)
    {
      plugin.Install(this);
    }

    private Handlers GetHandlers(string key)
    {
      try {
        return _eventPool[key];
      } catch {
        return _eventPool[key] = new Handlers();
      }
    }

    private T Decode<T>(string payload)
    {
      return JsonSerializer.Deserialize<T>(payload);
    }

    private IEventListener AddHandler(string key, Handler handler)
    {
      var id = _id++;
      var listener = new EventListener(() => RemoveHandler(key, id));
      GetHandlers(key).Add(id, handler);

      return listener;
    }

    private IEventListener AddOnceHandler(string key, Handler handler)
    {
      var id = _id++;
      var listener = new EventListener(() => RemoveHandler(key, id));
      GetHandlers(key).Add(id, payload => {
        listener.Cancel();
        return handler(payload);
      });

      return listener;
    }

    private IEventListener AddGlobalHandler(Func<IEventArgs, Task> handler)
    {
      var id = _id++;
      var listener = new EventListener(() => _globalHandlers.Remove(id));
      _globalHandlers.Add(id, args => handler(args));

      return listener;
    }

    private void RemoveHandler(string key, int id)
    {
      GetHandlers(key).Remove(id);
    }

    private Task HandleEvent(string key, string payload)
    {
      var tasks = new List<Task>();
      var handlers = GetHandlers(key).Values.ToList();
      var globalHandlers = _globalHandlers.Values.ToList();
      var eventArgs = new EventArgs() {
        Key = key,
        Payload = payload,
        HandlerCount = handlers.Count(),
      };

      foreach(var handler in handlers) {
        tasks.Add(handler(payload));
      }
      foreach (var handler in globalHandlers) {
        tasks.Add(handler(eventArgs));
      }

      return Task.WhenAll(tasks);
    }

    public IEventListener All(Func<IEventArgs, Task> handler)
    {
      return AddGlobalHandler(handler);
    }

    public IEventListener All(Action<IEventArgs> handler)
    {
      return All(args => Task.Run(() => handler(args)));
    }

    public IEventListener On<T>(string key, Func<T, Task> handler)
    {
      return AddHandler(key, payload => handler(Decode<T>(payload)));
    }

    public IEventListener On(string key, Func<Task> handler)
    {
      return On<string>(key, _ => handler());
    }

    public IEventListener On<T>(string key, Action<T> handler)
    {
      return On<T>(key, payload => Task.Run(() => handler(payload)));
    }

    public IEventListener On(string key, Action handler)
    {
      return On<string>(key, _ => Task.Run(() => handler()));
    }

    public IEventListener Once<T>(string key, Func<T, Task> handler)
    {
      return AddOnceHandler(key, payload => handler(Decode<T>(payload)));
    }

    public IEventListener Once(string key, Func<Task> handler)
    {
      return Once<string>(key, _ => handler());
    }

    public IEventListener Once<T>(string key, Action<T> handler)
    {
      return Once<T>(key, payload => Task.Run(() => handler(payload)));
    }

    public IEventListener Once(string key, Action handler)
    {
      return Once<string>(key, _ => Task.Run(handler));
    }

    public Task EmitAsync(string key)
    {
      return HandleEvent(key, "null");
    }

    public Task EmitAsync<T>(string key, T payload)
    {
      return HandleEvent(key, JsonSerializer.Serialize(payload));
    }

    public void Emit(string key)
    {
      EmitAsync(key).GetAwaiter().GetResult();
    }

    public void Emit<T>(string key, T payload)
    {
      EmitAsync<T>(key, payload).GetAwaiter().GetResult();
    }
  }
}
