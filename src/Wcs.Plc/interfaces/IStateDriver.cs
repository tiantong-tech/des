using System.Threading.Tasks;

namespace Wcs.Plc
{
  public interface IStateDriver
  {
    void BeforeMessage(IState state);

    void HandleStateSetKey(string key);

    void HandleStateSetLength(int length);

    //

    IStateDriver SetKey(string key);

    IStateDriver SetLength(int length);

    Task SetWord(int data);

    Task SetWords(string data);

    Task SetBit(bool data);

    Task SetBits(string data);

    Task<int> GetWord();

    Task<string> GetWords();

    Task<bool> GetBit();

    Task<string> GetBits();
  }
}
