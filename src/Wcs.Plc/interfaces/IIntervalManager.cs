using System.Threading.Tasks;

namespace Wcs.Plc
{
  public interface IIntervalManager
  {
    int Add(IInterval interval);

    Task RemoveAsync(int id);

    void Remove(int id);

    bool IsRunning();

    IIntervalManager Start();

    Task StopAsync();

    void Stop();

    Task ClearAsync();

    void Clear();

    Task WaitAsync();

    void Wait();
  }
}
