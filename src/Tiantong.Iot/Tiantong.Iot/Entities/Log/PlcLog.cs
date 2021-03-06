using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Iot.Entities
{
  [Table("plc_logs")]
  public class PlcLog
  {
    [Key]
    public int id { get; set; }

    public int plc_id { get; set; }

    public string message { get; set; }

    public DateTime created_at { get; set; } = DateTime.Now;
  }
}
