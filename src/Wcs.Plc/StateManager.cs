using System.Collections.Generic;

namespace Wcs.Plc
{
  using States = Dictionary<string, IState>;

  public class StateManager : IStateManager
  {
    private IContainer _container;

    public States States { get; } = new States();

    public string Name { get; set; }

    public StateManager(IContainer container)
    {
      _container = container;
    }

    public IStateManager SetName(string name)
    {
      Name = name;

      return this;
    }

    public IStateBit Bit(string key)
    {
      var state = new StateBit(_container) {
        Name = Name,
        Key = key,
        Length = 1,
      };

      States.Add(Name, state);

      return state;
    }

    public IStateBits Bits(string key, int length = 1)
    {
      var state = new StateBits(_container) {
        Name = Name,
        Key = key,
        Length = length,
      };

      States.Add(Name, state);

      return state;
    }

    public IStateWord Word(string key)
    {
      var state = new StateWord(_container) {
        Name = Name,
        Key = key,
        Length = 1,
      };

      States.Add(Name, state);

      return state;
    }

    public IStateWords Words(string key, int length = 1)
    {
      var state = new StateWords(_container) {
        Name = Name,
        Key = key,
        Length = length,
      };

      States.Add(Name, state);

      return state;
    }
  }
}
