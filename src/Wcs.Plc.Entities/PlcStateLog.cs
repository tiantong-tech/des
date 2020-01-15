using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wcs.Plc.Entities
{
  [Table("plc_state_logs")]
  public class PlcStateLog
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

    [Column("name")]
    [Required]
    public string Name { get; set; }

    [Column("key")]
    [Required]
    public string Key { get; set; }

    [Column("length")]
    [Required]
    public int Length { get; set; }

    [Column("value")]
    [Required]
    public string Value { get; set; }

    [Column("created_at")]
    [Required]
    public DateTime CreatedAt { get; set; }

    public PlcStateLog()
    {
      CreatedAt = DateTime.Now;
    }
  }
}
