using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Wcs.Plc
{
  using Tasks = List<Task>;
  using Intervals = Dictionary<int, IInterval>;

  public class IntervalManager : IIntervalManager
  {
    private int _id = 0;

    private Intervals intervals = new Intervals();

    public int Add(IInterval interval)
    {
      var id = _id++;
      intervals.Add(id, interval);

      return id;
    }

    public async Task RemoveAsync(int id)
    {
      var interval = intervals[id];

      if (interval.IsRunning()) {
        await interval.StopAsync();
      }

      intervals.Remove(id);
    }

    public void Remove(int id)
    {
      RemoveAsync(id).GetAwaiter().GetResult();
    }

    public bool IsRunning()
    {
      foreach (var interval in intervals.Values) {
        if (interval.IsRunning()) {
          return true;
        }
      }

      return false;
    }

    public IIntervalManager Start()
    {
      foreach (var interval in intervals.Values) {
        interval.Start();
      }

      return this;
    }

    public Task StopAsync()
    {
      var tasks = new Tasks();

      foreach (var interval in intervals.Values) {
        if (interval.IsRunning()) {
          tasks.Add(interval.StopAsync());
        }
      }

      return Task.WhenAll(tasks);
    }

    public void Stop()
    {
      StopAsync().GetAwaiter().GetResult();
    }

    public Task WaitAsync()
    {
      var tasks = new Tasks();

      foreach (var interval in intervals.Values) {
        tasks.Add(interval.WaitAsync());
      }

      return Task.WhenAll(tasks);
    }

    public void Wait()
    {
      WaitAsync().GetAwaiter().GetResult();
    }

    public async Task ClearAsync()
    {
      await StopAsync();
      intervals = new Intervals();
    }

    public void Clear()
    {
      ClearAsync().GetAwaiter().GetResult();
    }
  }
}
