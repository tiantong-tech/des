using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Wcs.Plc.Database;
using Wcs.Plc.Entities;
using Wcs.Plc.DB.Sqlite;

namespace Wcs.Plc
{
  public class Plc : IPlc
  {
    public PlcConnection Connection { get; set; } = new PlcConnection();

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
      UseStateLogger();
    }

    //

    private void UseContainer()
    {
      Container = new Container();
      Container.Plc = this;
      Container.Event = new Event();
      Container.IntervalManager = new IntervalManager();
      Container.StateManager = new StateManager(Container);
    }

    protected virtual void UseEventLogger()
    {
      var db = ResolveDbContext();
      var logger = new EventLogger(db);

      _ = logger.StartAsync();
      Container.Event.Use(logger);
    }

    /// <smmary>
    ///   <para>
    ///     将 StateLogger 添加至 Container 中，并开始执行记录
    ///   </para>
    /// </summary>
    protected virtual void UseStateLogger()
    {
      var db = ResolveDbContext();
      var logger = new StateLogger(db, Connection);

      Container.StateLogger = logger;
      _ = logger.StartAsync();
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

    /// <summary>
    ///   <para>
    ///     处理 PlcConnection 信息
    ///     通过 Id 或 Name，则查询 PlcConnection 信息
    ///     若通过 Name 无法找到对应信息，则根据当前信息创建 PlcConnection
    ///   </para>
    /// </summary>
    public void ResolvePlcConnection()
    {
      var db = ResolveDbContext();
      var id = Connection.Id;
      var name = Connection.Name;

      if (id != 0) {
        var conn = db.PlcConnections.SingleOrDefault(item => item.Id == id);

        if (conn != null) {
          Connection = conn;
        }
      } else if (name != null) {
        var conn = db.PlcConnections.SingleOrDefault(item => item.Name == name);

        if (conn == null) {
          db.PlcConnections.Add(Connection);
        } else {
          Connection.Id = conn.Id;
          conn.Host = Connection.Host;
          conn.Port = Connection.Port;
          conn.Model = Connection.Model;
        }

        db.SaveChanges();
      }
    }

    //

    public IPlcWorker Id(int id)
    {
      Connection.Id = id;

      return this;
    }

    public IPlcWorker Model(string model)
    {
      Connection.Model = model;

      return this;
    }

    public IPlcWorker Name(string name)
    {
      Connection.Name = name;

      return this;
    }

    public IPlcWorker Host(string host)
    {
      Connection.Host = host;

      return this;
    }

    public IPlcWorker Port(string port)
    {
      Connection.Port = port;

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
      ResolvePlcConnection();

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
