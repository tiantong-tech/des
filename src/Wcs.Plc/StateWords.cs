using System.Threading.Tasks;

namespace Wcs.Plc
{
  public class StateWords : State<string>, IStateWords
  {
    protected override Task<string> HandleGet()
    {
      return Driver.GetWords();
    }

    protected override Task HandleSet(string data)
    {
      return Driver.SetWords(data);
    }
  }
}
