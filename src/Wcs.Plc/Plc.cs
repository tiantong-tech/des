using System;
using System.Threading;
using System.Threading.Tasks;
using Wcs.Plc.Database;
using Wcs.Plc.DB.Sqlite;

namespace Wcs.Plc
{
  public class Plc : IPlc
  {
    public IContainer Container { get; set; }

    public IEvent Event
    {
      get => Container.Event;
    }

    public IIntervalManager IntervalManager
    {
      get => Container.IntervalManager;
    }

    public IStateManager StateManager
    {
      get => Container.StateManager;
    }

    private Task _task;

    private CancellationTokenSource _tokenSource;

    public Plc()
    {
      UseContainer();
      UseStateDriver();
      UseDbContext();
      UseEventLogger();
    }

    //

    private void UseContainer()
    {
      Container = new Container();
      Container.Event = new Event();
      Container.IntervalManager = new IntervalManager();
      Container.StateManager = new StateManager(Container);
    }

    protected virtual void UseEventLogger()
    {
      var db = ResolveDbContext();
      var logger = new EventLogger(db);

      logger.Start();
      Container.Event.Use(logger);
    }

    protected virtual void UseStateDriver()
    {
      Container.StateDriver = new StateTestDriver();
    }

    protected virtual void UseDbContext()
    {
      Container.DbContext = typeof(SqliteDbContext);
    }

    public virtual DbContext ResolveDbContext()
    {
      var instance = Activator.CreateInstance(Container.DbContext) as SqliteDbContext;

      return instance;
    }

    //

    public IPlcWorker Mode(string mode)
    {
      return this;
    }

    public IPlcWorker Name(string name)
    {
      return this;
    }

    public IPlcWorker Host(string host)
    {
      return this;
    }

    public IPlcWorker Port(string port)
    {
      return this;
    }

    public IStateManager State(string name)
    {
      StateManager.Name = name;

      return StateManager;
    }

    //

    public IStateWord Word(string name)
    {
      return StateManager.States[name].Convert<IStateWord>();
    }

    public IStateWords Words(string name)
    {
      return StateManager.States[name].Convert<IStateWords>();
    }

    public IStateBit Bit(string name)
    {
      return StateManager.States[name].Convert<IStateBit>();
    }

    public IStateBits Bits(string name)
    {
      return StateManager.States[name].Convert<IStateBits>();
    }

    //

    public void On<T>(string key, Func<T, Task> handler)
    {
      Event.On<T>(key, handler);
    }

    public void On(string key, Func<Task> handler)
    {
      Event.On(key, handler);
    }

    public void On<T>(string key, Action<T> handler)
    {
      Event.On<T>(key, handler);
    }

    public void On(string key, Action handler)
    {
      Event.On(key, handler);
    }

    //

    public IComparableWatcher<T> CreateComparableWatcher<T>(string key) where T : IComparable<T>
    {
      var watcher = new ComparableWatcher<T>(Event);
      var state = StateManager.States[key].Convert<IState<T>>();
      var hook = state.AddGetHook(value => watcher.Handle(value));

      return watcher;
    }

    private IWatcher<T> CreateWatcher<T>(string key)
    {
      var watcher = new Watcher<T>(Event);
      var state = StateManager.States[key].Convert<IState<T>>();
      var hook = state.AddGetHook(value => watcher.Handle(value));

      return watcher;
    }

    public IWatcher<T> Watch<T>(string key, T value) where T : IComparable<T>
    {
      return CreateComparableWatcher<T>(key).When(data => data.CompareTo(value) == 0);
    }

    public IWatcher<T> Watch<T>(string key, string opt, T value) where T : IComparable<T>
    {
      return CreateComparableWatcher<T>(key).When(opt, value).Event(key);
    }

    public IWatcher<T> Watch<T>(string key, Func<T, bool> handler)
    {
      return CreateWatcher<T>(key).When(handler);
    }

    //

    private async Task RunTask()
    {
      IntervalManager.Start();

      while (!_tokenSource.Token.IsCancellationRequested) {
        await Task.Delay(5000, _tokenSource.Token);
      }

      await IntervalManager.WaitAsync();
    }

    public Task RunAsync()
    {
      _tokenSource = new CancellationTokenSource();
      _task = RunTask().ContinueWith(_ => {
        _task = null;
        _tokenSource = null;
      });

      return _task;
    }

    public void Run()
    {
      RunAsync().GetAwaiter().GetResult();
    }

    public async Task StopAsync()
    {
      await IntervalManager.StopAsync();

      if (_tokenSource != null) {
        _tokenSource.Cancel();
      }
      if (_task != null) {
        await _task;
      }
    }

    public void Stop()
    {
      StopAsync().GetAwaiter().GetResult();
    }
  }
}
