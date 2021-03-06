using Microsoft.AspNetCore.Mvc;
using Midos.Domain;
using System.Linq;
using System.Threading.Tasks;

namespace Namei.Wcs.Api
{
  public class DoorTaskWebController: BaseController
  {
    private IRcsService _rcs;

    private DomainContext _domain;

    public DoorTaskWebController(
      IRcsService rcs,
      DomainContext domain
    ) {
      _rcs = rcs;
      _domain = domain;
    }

    [HttpPost("/door-tasks/search")]
    public IPagination<RcsDoorTask, string> Search([FromBody] QueryParams param)
    {
      var query = _domain.Set<RcsDoorTask>().AsQueryable();

      if (param.Query != null && param.Query != "") {
        query = query.Where(task =>
          task.Id.Contains(param.Query) ||
          task.DoorId.Contains(param.Query)
        );
      }

      return query
        .OrderByDescending(task => task.Status == RcsDoorTaskStatus.Requested)
        .ThenByDescending(task => task.Status == RcsDoorTaskStatus.Entered)
        .ThenByDescending(task => task.RequestedAt)
        .ThenByDescending(task => task.Id)
        .Paginate<RcsDoorTask, string>(param);
    }

    [HttpPost("/rcs/tasks/create")]
    public Task<RcsTaskCreateResult> HandleTaskCreate([FromBody] RcsTaskCreateParams param)
    {
      return _rcs.CreateTask(param);
    }

    [HttpPost("/rcs/tasks/continue")]
    public Task<RcsTaskCreateResult> HandleTaskContinue([FromBody] RcsTaskContinueParams param)
      => _rcs.ContinueTask(param);

    [HttpPost("/rcs/tasks/cancel")]
    public Task<RcsTaskCancelResult> HandleTaskCancel([FromBody] RcsTaskCancelParams param)
      => _rcs.CancelTask(param);

    public class RcsDoorNotifyParams
    {
      public string DoorId { get; set; }

      public string Uuid { get; set; }
    }

    [HttpPost("/rcs/doors/notify")]
    public async Task<object> RcsDoorNotify([FromBody] RcsDoorNotifyParams param)
    {
      await _rcs.NotifyDoorOpened(param.DoorId, param.Uuid);

      return NotifyResult.FromVoid().Success("任务已通知");
    }

    public class PublishDoorsMessageParams
    {
      public string door_id { get; set; }

      public string message { get; set; }
    }

    // Todo Remove

    [HttpPost]
    [Route("/test/doors/publish-message")]
    public object PublishDoorsMessage([FromBody] PublishDoorsMessageParams param)
    {
      if (param.message == "requested.open") {
        _domain.Publish(
          RcsDoorEvent.Request,
          RcsDoorEvent.From(
            uuid: "A0001",
            doorId: param.door_id
          )
        );
      } else if (param.message  == "requested.close") {
        _domain.Publish(
          RcsDoorEvent.Leave,
          RcsDoorEvent.From(
            uuid: "A0001",
            doorId: param.door_id
          )
        );
      } else if (param.message == "opened") {
        _domain.Publish(
          WcsDoorEvent.Opened,
          WcsDoorEvent.From(param.door_id)
        );
      } else if (param.message == "closed") {
        _domain.Publish(
          WcsDoorEvent.Closed,
          WcsDoorEvent.From(param.door_id)
        );
      }

      return NotifyResult.FromVoid().Success("指令已发送");
    }
  }
}
