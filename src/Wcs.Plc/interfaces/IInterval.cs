using System;
using System.Threading.Tasks;

namespace Wcs.Plc
{
  public interface IInterval
  {
    IInterval SetTime(int time);

    IInterval SetTimes(int times);

    IInterval SetHandler(Action handler);

    IInterval SetHandler(Func<Task> handler);

    bool IsRunning();

    IInterval Start();

    Task StopAsync();

    void Stop();

    Task WaitAsync();

    void Wait();
  }
}
