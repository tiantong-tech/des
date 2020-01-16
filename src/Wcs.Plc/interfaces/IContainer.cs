using System;
using Wcs.Plc.Database;

namespace Wcs.Plc
{
  public interface IContainer
  {
    IPlc Plc { get; set; }

    IEvent Event { get; set; }

    Type EventLogger { get; set; }

    IStateDriver StateDriver { get; set; }

    IStateManager StateManager { get; set; }

    IIntervalManager IntervalManager { get; set; }

    Type DbContext { get; set; }

    StateLogger StateLogger { get; set; }
  }
}
