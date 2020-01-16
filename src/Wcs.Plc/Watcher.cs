using System;
using System.Threading.Tasks;

namespace Wcs.Plc
{
  public class Watcher<T> : IWatcher<T>
  {
    private IEvent _event;

    private Action _cancel;

    private Action<T> _emmiter;

    protected Func<T, bool> _when;

    public Watcher(IEvent event_)
    {
      _event = event_;
    }

    public void Handle(T value)
    {
      if (_when(value)) {
        _emmiter(value);
      }
    }

    public void OnCancel(Action cancel)
    {
      _cancel = cancel;
    }

    public void Cancel()
    {
      _cancel();
    }

    public IWatcher<T> When(Func<T, bool> comparer)
    {
      _when = comparer;

      return this;
    }

    public IWatcher<T> Event(string key)
    {
      _emmiter = value => _event.EmitAsync(key, value);

      return this;
    }

    public IWatcher<T> EventVoid(string key)
    {
      _emmiter = _ => _event.EmitAsync(key);

      return this;
    }

    public IWatcher<T> Event<R>(string key, R payload)
    {
      _emmiter = _ => _event.EmitAsync<R>(key, payload);

      return this;
    }

    public IWatcher<T> Event(string key, Func<T, T> handler)
    {
      _emmiter = value => _event.EmitAsync(key, handler(value));

      return this;
    }

    public IWatcher<T> Event<R>(string key, Func<T, R> handler)
    {
      _emmiter = value => _event.EmitAsync<R>(key, handler(value));

      return this;
    }
  }

  public class ComparableWatcher<T> : Watcher<T>, IComparableWatcher<T> where T : IComparable<T>
  {
    public ComparableWatcher(IEvent event_) : base(event_)
    {

    }

    public IWatcher<T> When(string opt, T value)
    {
      switch (opt)
      {
        case ">":
          _when = data => data.CompareTo(value) > 0;
          break;
        case "<":
          _when = data => data.CompareTo(value) < 0;
          break;
        case ">=":
          _when = data => data.CompareTo(value) >= 0;
          break;
        case "<=":
          _when = data => data.CompareTo(value) <= 0;
          break;
        case "!=":
          _when = data => data.CompareTo(value) != 0;
          break;
        default:
          _when = data => data.CompareTo(value) == 0;
          break;
      }

      return this;
    }
  }
}
