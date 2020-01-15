using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wcs.Plc.Entities
{
  [Table("plc_connections")]
  public class PlcConnection
  {
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("model")]
    [Required]
    public string Model { get; set; }

    [Column("name")]
    [Required]
    public string Name { get; set; }

    [Column("host")]
    public string Host { get; set; }

    [Column("port")]
    public string Port { get; set; }

    [Column("created_at")]
    [Required]
    public DateTime CreatedAt { get; set; }
  }
}
