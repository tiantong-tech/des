using System;
using System.Threading;
using System.Threading.Tasks;

namespace Wcs.Plc
{
  public class Interval : IInterval
  {
    public int Times { get; protected set; } = 0;

    private int _time = 0;

    private Func<Task> _handler;

    private Task _task;

    private CancellationTokenSource _tokenSource;

    public Interval()
    {

    }

    ~Interval()
    {
      if (_task != null) {
        Stop();
      }
    }

    public Interval(Func<Task> handler, int time = 500)
    {
      SetTime(time);
      SetHandler(handler);
    }

    public Interval(Action handler, int time = 500)
    {
      SetTime(time);
      SetHandler(handler);
    }

    public IInterval SetTime(int time)
    {
      _time = time;

      return this;
    }

    public IInterval SetTimes(int times)
    {
      Times = times;

      return this;
    }

    public IInterval SetHandler(Action handler)
    {
      _handler = () => Task.Run(handler);

      return this;
    }

    public IInterval SetHandler(Func<Task> handler)
    {
      _handler = handler;

      return this;
    }

    public bool IsRunning()
    {
      return _task != null;
    }

    private async Task RunTask()
    {
      var times = 0;
      while (!_tokenSource.Token.IsCancellationRequested) {
        if (Times != 0) {
          if (times < Times) times++;
          else break;
        }
        await _handler();
        try {
          await Task.Delay(_time, _tokenSource.Token);
        } catch (TaskCanceledException) {}
      }
    }

    public IInterval Start()
    {
      _tokenSource = new CancellationTokenSource();
      _task = RunTask();

      return this;
    }

    public async Task WaitAsync()
    {
      if (_task != null) {
        await _task;
        _task = null;
        _tokenSource = null;
      }
    }

    public void Wait()
    {
      WaitAsync().GetAwaiter().GetResult();
    }

    public async Task StopAsync()
    {
      if (_tokenSource != null) {
        _tokenSource.Cancel();
      }
      if (_task != null) {
        await _task;
      }

      _task = null;
      _tokenSource = null;
    }

    public void Stop()
    {
      StopAsync().GetAwaiter().GetResult();
    }
  }
}
