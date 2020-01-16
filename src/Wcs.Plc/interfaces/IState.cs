using System;
using System.Threading.Tasks;

namespace Wcs.Plc
{
  public interface IState
  {
    IContainer Container { get; }

    void ResolveDriver();

    String Name { get; set; }

    String Key { get; set; }

    int Length { get; set; }

    IState Collect(int interval = 1000);

    Task UncollectAsync();

    void Uncollect();

    S Convert<S>() where S : IState;
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
