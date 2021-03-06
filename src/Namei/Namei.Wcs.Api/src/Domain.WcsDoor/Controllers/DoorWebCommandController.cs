using Microsoft.AspNetCore.Mvc;

namespace Namei.Wcs.Api
{
  public class DoorWebCommandController: BaseController
  {
    private IWcsDoorFactory _doors;

    public DoorWebCommandController(IWcsDoorFactory doors)
    {
      _doors = doors;
    }

    public class OpenParams
    {
      public string door_id { get; set; }

      public string command { get; set; }
    }

    [HttpPost("/doors/control")]
    public object Open([FromBody] OpenParams param)
    {
      var message = "指令未识别";

      if (param.command == "open") {
        _doors.Get(param.door_id).Open();
      } else if (param.command == "close") {
        _doors.Get(param.door_id).Close();
      }

      return NotifyResult.FromVoid()
        .Success(message);
    }
  }
}
