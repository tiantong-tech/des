using System.Collections.Concurrent;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Wcs.Plc.Entities;
using Wcs.Plc.Database;

namespace Wcs.Plc
{
  public class EventLogger : IEventPlugin
  {
    private DbContext _db;

    /// <summary>
    ///   <para>
    ///     正在缓存和执行的任务
    ///   </para>
    /// </summary>
    private Task _task;

    private List<EventLog> _eventLogs = new List<EventLog>();

    public int LogInterval = 500;

    public EventLogger(DbContext db)
    {
      _db = db;
    }

    /// <summary>
    ///   等待任务缓存并执行
    /// </summary>
    /// 执行任务时将 lock <see ref="_eventLogs" /> 以避免问题
    public async Task StartAsync()
    {
      var time = Math.Max(LogInterval, 1);

      Func<Task> handleTask = async () => {
        await Task.Delay(time);

        if (_eventLogs.Count > 0) {
          EventLog[] logs;

          lock (_eventLogs) {
            logs = _eventLogs.ToArray();
            _eventLogs.Clear();
          }

          _db.EventLogs.AddRange(logs);
          _db.SaveChanges();
        }
      };

      while (true) {
        _task = handleTask();
        await _task;
      }
    }

    public void Install(IEvent _event)
    {
      _event.All(async args => {
        var log = new EventLog {
          Key = args.Key,
          Payload = args.Payload,
          HandlerCount = args.HandlerCount,
        };

        lock (_eventLogs) {
          _eventLogs.Add(log);
        }

        await _task;
      });
    }
  }
}
