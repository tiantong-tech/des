using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace Namei.Wcs.Api
{
  public class DoorPlcCommandController: BaseController
  {
    private ICapPublisher _cap;

    public DoorPlcCommandController(ICapPublisher cap)
    {
      _cap = cap;
    }

    public class DoorStateChangedParams
    {
      public string door_id { get; set; }

      public string value { get; set; }
    }

    [HttpPost("/doors/state/changed")]
    public object DoorStateChanged([FromBody] DoorStateChangedParams param)
    {
      var message = "指令未识别";

      if (param.value == "12") {
        message = "正在处理开门完成指令";
        _cap.Publish(WcsDoorEvent.Opened, WcsDoorEvent.From(param.door_id));
      } else if (param.value == "22") {
        message = "正在处理关门完成指令";
        _cap.Publish(WcsDoorEvent.Closed, WcsDoorEvent.From(param.door_id));
      }

      return NotifyResult.FromVoid()
        .Success(message);
    }
  }
}
