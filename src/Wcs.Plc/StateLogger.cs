using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Wcs.Plc.Database;
using Wcs.Plc.Entities;

namespace Wcs.Plc
{
  public class StateLogger : IStatePlugin
  {
    private DbContext _db;

    private PlcConnection _connection;

    public Task RunningTask;

    private List<PlcStateLog> _plcStateLogs = new List<PlcStateLog>();

    /// <summary>
    ///   <para>
    ///     logger 记录周期
    ///   </para>
    /// </summary>
    public int LogInterval = 500;

    public StateLogger(DbContext db, PlcConnection connection)
    {
      _db = db;
      _connection = connection;
    }

    public async Task StartAsync()
    {
      var time = Math.Max(LogInterval, 1);

      Func<Task> handleTask = async () => {
        await Task.Delay(time);

        if (_plcStateLogs.Count > 0) {
          PlcStateLog[] logs;

          lock (_plcStateLogs) {
            logs = _plcStateLogs.ToArray();
            _plcStateLogs.Clear();
          }

          _db.PlcStateLogs.AddRange(logs);
          _db.SaveChanges();
        }
      };

      while (true) {
        RunningTask = handleTask();
        await RunningTask;
      }
    }

    private void AddLog(PlcStateLog log)
    {
      lock (_plcStateLogs) {
        _plcStateLogs.Add(log);
      }
    }

    public void Install<T>(IState<T> state)
    {
      state.AddSetHook(async value => {
        AddLog(new PlcStateLog {
          PlcId = _connection.Id,
          Operation = "write",
          Key = state.Key,
          Name = state.Name,
          Length = state.Length,
          Value = JsonSerializer.Serialize(value),
        });

        await RunningTask;
      });

      state.AddGetHook(async value => {
        AddLog(new PlcStateLog {
          PlcId = _connection.Id,
          Operation = "read",
          Key = state.Key,
          Name = state.Name,
          Length = state.Length,
          Value = JsonSerializer.Serialize(value),
        });

        await RunningTask;
      });
    }
  }
}
