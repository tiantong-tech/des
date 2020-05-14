using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;

namespace Tiantong.Iot.Api
{
  public class PlcManagerService : BackgroundService
  {
    private PlcManager _plcManager;

    public PlcManagerService(PlcManager plcManager)
    {
      _plcManager = plcManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      
      while (!stoppingToken.IsCancellationRequested) {
        await Task.Delay(100);
      }

      await _plcManager.Stop().WaitAsync();
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
      await base.StopAsync(stoppingToken);
    }
  }

  public class PlcManager
  {
    private Dictionary<int, IPlcWorker> _plcById = new Dictionary<int, IPlcWorker>();

    private Dictionary<string, IPlcWorker> _plcByName = new Dictionary<string, IPlcWorker>();

    public IPlcWorker Get(int id)
    {
      return _plcById[id];
    }

    public IPlcWorker Get(string name)
    {
      return _plcByName[name];
    }

    public bool Run(PlcWorker worker)
    {
      if (_plcById.ContainsKey(worker._id)) {
        return false;
      } else {
        _plcById[worker._id] = _plcByName[worker._name] = worker;
        worker.Run();

        return true;
      }
    }

    public bool Stop(int id)
    {
      if (_plcById.ContainsKey(id)) {
        var worker = _plcById[id];

        worker.Stop().WaitAsync()
          .ContinueWith(task => {
            try {
              task.GetAwaiter().GetResult();
            } finally {
              _plcById.Remove(worker._id);
              _plcByName.Remove(worker._name);
            }
          });

        return true;
      } else {
        return false;
      }
    }

    public PlcManager Stop()
    {
      foreach (var plc in _plcById.Values) {
        plc.Stop();
      }

      return this;
    }

    public async Task WaitAsync()
    {
      await Task.WhenAll(_plcById.Values.Select(plc => plc.WaitAsync()));
    }
  }

}
