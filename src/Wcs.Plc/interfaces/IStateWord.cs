using System.Threading.Tasks;

namespace Wcs.Plc
{
  public interface IStateWord : IState<int>
  {
    IStateWord Heartbeat(int time = 1000);

    Task UnheartbeatAsync();

    void Unheartbeat();
  }
}
