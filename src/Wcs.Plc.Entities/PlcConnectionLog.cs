using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wcs.Plc.Entities
{
  [Table("plc_connection_logs")]
  public class PlcConnectionLog
  {
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("plc_id")]
    [Required]
    public int PlcId { get; set; }

    [Column("operation")]
    [Required]
    public string Operation { get; set; }

    [Column("created_at")]
    [Required]
    public DateTime CreatedAt { get; set; }
  }
}
