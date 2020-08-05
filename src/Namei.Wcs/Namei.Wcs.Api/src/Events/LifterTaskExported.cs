namespace Namei.Wcs.Api
{
  public class LifterTaskExportedEvent
  {
    public const string Message = "lifter.task.exported";

    public string LifterId { get; set; }

    public string Floor { get; set; }

    public LifterTaskExportedEvent(string lifterId, string floor)
    {
      Floor = floor;
      LifterId = lifterId;
    }
  }
}
