namespace Namei.Wcs.Api
{
  public class LifterTaskImported
  {
    public const string Message = "lifter.task.imported";

    public string LifterId { get; set; }

    public string Floor { get; set; }

    public string Barcode { get; set; }

    public string Destination { get; set; }

    private LifterTaskImported() {}

    public static LifterTaskImported From(
      string lifterId,
      string floor,
      string barcode = null,
      string destination = null
    ) => new LifterTaskImported {
      LifterId = lifterId,
      Floor = floor,
      Barcode = barcode,
      Destination = destination
    };
  }
}
