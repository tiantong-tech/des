using System.Threading.Tasks;

namespace Wcs.Plc
{
  public class StateBits : State<string>, IStateBits
  {
    protected override Task<string> HandleGet()
    {
      return Driver.GetBits();
    }

    protected override Task HandleSet(string data)
    {
      return Driver.SetBits(data);
    }
  }
}
