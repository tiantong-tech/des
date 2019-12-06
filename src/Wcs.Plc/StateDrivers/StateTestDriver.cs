using System.Threading.Tasks;
using System.Collections.Generic;

namespace Wcs.Plc
{
  using BitStore = Dictionary<string, bool>;
  using BitsStore = Dictionary<string, string>;
  using WordStore = Dictionary<string, int>;
  using WordsStore = Dictionary<string, string>;

  public class StateTestDriver : IStateDriver
  {
    private string Key;

    private int Length;

    public BitStore BitStore = new BitStore();

    public BitsStore BitsStore = new BitsStore();

    public WordStore WordStore = new WordStore();

    public WordsStore WordsStore = new WordsStore();

    public void BeforeMessage(IState state)
    {
      SetKey(state.Key);
      SetLength(state.Length);
    }

    public void HandleStateSetKey(string key)
    {

    }

    public void HandleStateSetLength(int length)
    {

    }

    public IStateDriver SetKey(string key)
    {
      Key = key;

      return this;
    }

    public IStateDriver SetLength(int length)
    {
      Length = length;

      return this;
    }

    public Task SetWord(int data)
    {
      WordStore.Add(Key, data);

      return Task.Delay(0);
    }

    public Task SetWords(string data)
    {
      WordsStore.Add(Key, data);

      return Task.Delay(0);
    }

    public Task SetBit(bool data)
    {
      BitStore.Add(Key, data);

      return Task.Delay(0);
    }

    public Task SetBits(string data)
    {
      BitsStore.Add(Key, data);

      return Task.Delay(0);
    }

    public async Task<int> GetWord()
    {
      await Task.Delay(0);
      return WordStore[Key];
    }

    public async Task<string> GetWords()
    {
      await Task.Delay(0);
      return WordsStore[Key];
    }

    public async Task<bool> GetBit()
    {
      await Task.Delay(0);
      return BitStore[Key];
    }

    public async Task<string> GetBits()
    {
      await Task.Delay(0);
      return BitsStore[Key];
    }
  }
}
