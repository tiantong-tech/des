using System;
using System.ComponentModel.DataAnnotations;

namespace Wcs.Plc.Entities
{
  public class EventLog
  {
    public int Id { get; set; }

    [Required]
    public string Key { get; set; }

    [Required]
    public string Payload { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }
  }
}
