using System;
using System.Linq;
using System.Threading.Tasks;
using Wcs.Plc.Entities;

namespace Wcs.Plc
{
  public class Plc : IPlc
  {
    public EventPlugin EventLogger;

    public IStatePlugin StateLogger;

    public StateManager StateManager;

    public Event Event;

    public PlcConnection PlcConnection;

    public IntervalManager IntervalManager;

    public DatabaseProvider DatabaseProvider;

    public IStateClientProvider StateClientProvider;

    public Plc()
    {
      PlcConnection = new PlcConnection();
    }

    //

    private IStateClientProvider GetStateClientProvider()
    {
      switch (PlcConnection.Model) {
        case "S7200Smart": return new S7ClientProvider();
        case "test" : return new StateTestClientProvider();
        default: throw new Exception("plc model is not supporting");
      }
    }

    //

    public IPlc Id(int id)
    {
      PlcConnection.Id = id;

      return this;
    }

    public IPlc Model(string model)
    {
      PlcConnection.Model = model;

      return this;
    }

    public IPlc Name(string name)
    {
      PlcConnection.Name = name;

      return this;
    }

    public IPlc Host(string host)
    {
      PlcConnection.Host = host;
      
      return this;
    }

    public IPlc Port(int port)
    {
      PlcConnection.Port = port;

      return this;
    }

    //

    public virtual void ResolveDatabaseProvider()
    {
      DatabaseProvider = new DatabaseProvider();
      DatabaseProvider.Migrate();
    }

    public virtual void ResolveStateClientProvider()
    {
      StateClientProvider = GetStateClientProvider();
    }

    //

    public IPlc Build()
    {
      Event = new Event();
      IntervalManager = new IntervalManager();
      ResolveDatabaseProvider();
      ResolveStateClientProvider();
      ResolveEventLogger();
      ResolveStateLogger();
      ResolveStateManager();

      HandlePlcConnection();

      return this;
    }

    public virtual IPlc UseTest()
    {
      return Build();
    }

    public virtual IPlc UseS7200Smart(string host, int port = 102)
    {
      StateClientProvider = new S7ClientProvider();
      Model("S7200Smart").Host(host).Port(port).Build();

      return this;
    }

    public virtual void ResolveEventLogger()
    {
      EventLogger = new EventLogger(IntervalManager, DatabaseProvider.Resolve());

      Event.Use(EventLogger);
    }

    public virtual void ResolveStateLogger()
    {
      StateLogger = new StateLogger(IntervalManager, DatabaseProvider.Resolve(), PlcConnection);
    }

    public virtual void ResolveStateManager()
    {
      StateManager = new StateManager(Event, IntervalManager, StateClientProvider, StateLogger);
    }

    public virtual void HandlePlcConnection()
    {
      var id = PlcConnection.Id;
      var name = PlcConnection.Name;
      var db = DatabaseProvider.Resolve();

      if (id != 0) {
        var conn = db.PlcConnections.SingleOrDefault(item => item.Id == id);

        if (conn != null) {
          PlcConnection = conn;
        } else {
          throw new Exception($"PlcConnection Id({id}) does not existed");
        }
      } else if (name != null) {
        var conn = db.PlcConnections.SingleOrDefault(item => item.Name == name);

        if (conn == null) {
          db.PlcConnections.Add(PlcConnection);
        } else {
          conn.Model = PlcConnection.Model;
          conn.Host = PlcConnection.Host;
          conn.Port = PlcConnection.Port;
          PlcConnection = conn;
        }

        db.SaveChanges();
      } else {
        throw new Exception("Plc Connection Id or Name does not existed");
      }
    }

    public IStateManager State(string name)
    {
      StateManager.Name = name;

      return StateManager;
    }

    //

    public IStateBool Bool(string name)
    {
      return StateManager.States[name].ToStateBool();
    }

    public IStateInt Int(string name)
    {
      return StateManager.States[name].ToStateInt();
    }

    public IStateString String(string name)
    {
      return StateManager.States[name].ToStateString();
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

    public IPlc Start()
    {
      IntervalManager.Start();

      return this;
    }

    public IPlc Stop()
    {
      IntervalManager.Stop();

      return this;
    }

    public Task WaitAsync()
    {
      return IntervalManager.WaitAsync();
    }

    public void Wait()
    {
      IntervalManager.Wait();
    }

    public Task RunAsync()
    {
      return IntervalManager.RunAsync();
    }

    public void Run()
    {
      IntervalManager.Run();
    }
  }
}
