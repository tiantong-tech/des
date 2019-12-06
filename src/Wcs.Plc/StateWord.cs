using System.Threading.Tasks;

namespace Wcs.Plc
{
  public class StateWord : State<int>, IStateWord
  {
    protected int _heartbeatIntervalId;

    protected override Task<int> HandleGet()
    {
      return Driver.GetWord();
    }

    protected override Task HandleSet(int data)
    {
      return Driver.SetWord(data);
    }

    public IStateWord Heartbeat(int time = 1000)
    {
      var interval = new Interval();
      var times = 1;

      interval.SetTime(time);
      interval.SetHandler(() => SetAsync(times++));
      _heartbeatIntervalId = IntervalManager.Add(interval);

      return this;
    }

    public Task UnheartbeatAsync()
    {
      return IntervalManager.RemoveAsync(_heartbeatIntervalId);
    }

    public void Unheartbeat()
    {
      UnheartbeatAsync().GetAwaiter().GetResult();      
    }
  }
}
