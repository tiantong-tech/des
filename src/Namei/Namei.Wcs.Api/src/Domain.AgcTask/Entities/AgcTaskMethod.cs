using System.Collections.Generic;

namespace Namei.Wcs.Aggregates
{
  public static class AgcTaskMethod
  {
    public const string Move = "wcs.move";

    public const string MoveLock = "wcs.move.lock";

    public const string Lift = "wcs.lift";

    public const string Put = "wcs.put";

    public const string PutLock = "wcs.put.lock";

    public const string Carry = "wcs.carry";

    public const string CarryLock = "wcs.carry.lock";

    public static IEnumerable<string> Values
    {
      get {
        yield return Move;
        yield return MoveLock;
        yield return Lift;
        yield return Put;
        yield return PutLock;
        yield return Carry;
        yield return CarryLock;
      }
    }
  }
}
