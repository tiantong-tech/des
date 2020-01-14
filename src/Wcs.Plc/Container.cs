using System;
using Wcs.Plc.Database;

namespace Wcs.Plc
{
  public class Container: IContainer
  {
    static public IContainer GetTestContainer()
    {
      var container = new Container {
        Event = new Event(),
        StateDriver = new StateTestDriver(),
        IntervalManager = new IntervalManager()
      };

      container.StateManager = new StateManager(container);

      return container;
    }

    public IEvent Event { get; set; }

    public Type EventLogger { get; set; }

    public IStateDriver StateDriver { get; set; }

    public IStateManager StateManager { get; set; }

    public IIntervalManager IntervalManager { get; set; }

    public Type DbContext { get; set; }
  }
}
