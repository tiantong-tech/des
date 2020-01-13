using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wcs.Plc.Entities
{
  [Table("event_logs")]
  public class EventLog
  {
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("key")]
    public string Key { get; set; }

    [Column("payload")]
    [Required]
    public string Payload { get; set; }

    [Column("handler_count")]
    [Required]
    public int HandlerCount { get; set; }

    [Column("created_at")]
    [Required]
    public DateTime CreatedAt { get; set; }

    public EventLog()
    {
      CreatedAt = DateTime.Now;
    }
  }
}
