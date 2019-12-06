using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Wcs.Plc
{
  class Hooks<T> : Dictionary<int, Func<T, Task>> {};

  public abstract class State<T> : IState<T>
  {
    public string Key { get; private set; }

    public int Length { get; private set; }

    private Hooks<T> _gethooks = new Hooks<T>();

    private Hooks<T> _sethooks = new Hooks<T>();

    public IStateDriver Driver;

    public IIntervalManager IntervalManager;

    private int _id = 0;

    private int _collectIntervalId = 0;

    public void SetStateDriver(IStateDriver driver)
    {
      Driver = driver;
    }

    public void SetIntervalManager(IIntervalManager manager)
    {
      IntervalManager = manager;
    }

    //

    public void SetKey(string key)
    {
      Key = key;
      Driver.HandleStateSetKey(key);
    }

    public void SetLength(int length)
    {
      Length = length;
      Driver.HandleStateSetLength(length);
    }

    public IStateHook<T> AddSetHook(Func<T, Task> hook)
    {
      int id = _id++;
      _sethooks.Add(id, hook);

      return new StateHook<T>(() => RemoveSetHook(id));
    }

    public IStateHook<T> AddGetHook(Func<T, Task> hook)
    {
      int id = _id++;
      _gethooks.Add(id, hook);

      return new StateHook<T>(() => RemoveGetHook(id));
    }

    public IStateHook<T> AddSetHook(Action<T> hook)
    {
      return AddSetHook(data => Task.Run(() => hook(data)));
    }

    public IStateHook<T> AddGetHook(Action<T> hook)
    {
      return AddGetHook(data => Task.Run(() => hook(data)));
    }

    private void RemoveSetHook(int id)
    {
      _gethooks.Remove(id);
    }

    private void RemoveGetHook(int id)
    {
      _sethooks.Remove(id);
    }

    public IState Collect(int time = 1000)
    {
      var interval = new Interval();

      interval.SetTime(time);
      interval.SetHandler(GetAsync);

      _collectIntervalId = IntervalManager.Add(interval);

      return this;
    }

    public Task UncollectAsync()
    {
      return IntervalManager.RemoveAsync(_collectIntervalId);
    }

    public void Uncollect()
    {
      UncollectAsync().GetAwaiter().GetResult();
    }

    public async Task SetAsync(T data)
    {
      Driver.BeforeMessage(this);

      var tasks = new List<Task>();

      await HandleSet(data);

      foreach (var hook in _sethooks.Values) {
        tasks.Add(hook(data));
      }

      await Task.WhenAll(tasks);
    }

    public void Set(T data)
    {
      SetAsync(data).GetAwaiter().GetResult();
    }

    public async Task<T> GetAsync()
    {
      Driver.BeforeMessage(this);

      var tasks = new List<Task>();
      var data = await HandleGet();

      foreach (var hook in _gethooks.Values) {
        tasks.Add(hook(data));
      }
      await Task.WhenAll(tasks);

      return data;
    }

    public T Get()
    {
      return GetAsync().GetAwaiter().GetResult();
    }

    protected abstract Task<T> HandleGet();

    protected abstract Task HandleSet(T data);
  }
}
