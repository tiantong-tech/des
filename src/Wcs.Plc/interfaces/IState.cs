using System;
using System.Threading.Tasks;

namespace Wcs.Plc
{
  public interface IState
  {
    String Key { get; }

    int Length { get; }

    void SetStateDriver(IStateDriver driver);

    void SetIntervalManager(IIntervalManager manager);

    void SetKey(string key);

    void SetLength(int length = 1);

    IState Collect(int interval = 1000);

    Task UncollectAsync();

    void Uncollect();
  }

  public interface IState<T> : IState
  {
    IStateHook<T> AddGetHook(Action<T> hook);

    IStateHook<T> AddSetHook(Action<T> hook);

    Task SetAsync(T data);

    void Set(T data);

    Task<T> GetAsync();

    T Get();
  }
}
