using System;
using System.Threading.Tasks;

namespace Wcs.Plc
{
  public interface IEventPlugin
  {
    void Install(IEvent event_);
  }
}
