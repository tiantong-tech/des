using System;

namespace Wcs.Plc
{
  public class StateHook<T> : IStateHook<T>
  {
    private Action _cancel;

    public StateHook(Action cancel)
    {
      _cancel = cancel;
    }

    public void Cancel() => _cancel();
  }
}
