using System.Threading.Tasks;
using System.Collections.Generic;
using Wcs.Plc.Entities;
using Wcs.Plc.Database;

namespace Wcs.Plc
{
  public class EventLogger : IEventPlugin
  {
    private DbContext _db;

    private List<EventLog> _eventLogs = new List<EventLog>();

    public int LogInterval = 500;

    public EventLogger(DbContext db)
    {
      _db = db;
    }

    public void Start()
    {
      Task.Run(() => {
        while (true) {
          if (_eventLogs.Count > 0) {
            lock(_eventLogs) {
              _db.EventLogs.AddRange(_eventLogs);
              _eventLogs.Clear();
            }

            _db.SaveChanges();
          }

          Task.Delay(LogInterval).GetAwaiter().GetResult();
        }
      });
    }

    public void Install(IEvent _event)
    {
      _event.All(args => {
        var log = new EventLog {
          Key = args.Key,
          Payload = args.Payload,
          HandlerCount = args.HandlerCount,
        };

        lock(_eventLogs) {
          _eventLogs.Add(log);
        }
      });
    }
  }
}
