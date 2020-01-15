using System;
using System.Threading.Tasks;
using Wcs.Plc.Database;

namespace Wcs.Plc
{
  public interface IPlcWorker
  {
    IPlcWorker Id(int id);

    IPlcWorker Model(string key);

    IPlcWorker Name(string key);

    IPlcWorker Host(string host);

    IPlcWorker Port(string port);

    IStateManager State(string key);

    IStateWord Word(string key);

    IStateWords Words(string key);

    IStateBit Bit(string key);

    IStateBits Bits(string key);

    void On<T>(string key, Func<T, Task> handler);

    void On(string key, Func<Task> handler);

    void On<T>(string key, Action<T> handler);

    void On(string key, Action handler);

    IWatcher<T> Watch<T>(string key, Func<T, bool> comparer);

    IWatcher<T> Watch<T>(string key, T value) where T : IComparable<T>;

    IWatcher<T> Watch<T>(string key, string opt, T value) where T : IComparable<T>;

    Task RunAsync();

    void Run();

    Task StopAsync();

    void Stop();

    DbContext ResolveDbContext();
  }
}
