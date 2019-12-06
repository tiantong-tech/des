using System.Threading.Tasks;

namespace Wcs.Plc
{
  public class StateBit : State<bool>, IStateBit
  {
    protected override Task<bool> HandleGet()
    {
      return Driver.GetBit();
    }

    protected override Task HandleSet(bool data)
    {
      return Driver.SetBit(data);
    }
  }
}
