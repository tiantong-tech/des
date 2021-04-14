using System.Collections.Generic;

namespace Midos.Domain.Test
{
  public class TestEventPublisher: IEventPublisher
  {
    public static Dictionary<string, object> events
      = new Dictionary<string, object>();

    public void Publish(string name, object data)
    {
      events.Add(name, data);
    }
  }
}
