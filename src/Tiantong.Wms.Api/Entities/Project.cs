using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("projects")]
  public class Project : Entity
  {
    [Key]
    public int id { get; set; }

    public int warehouse_id { get; set; }

    public string number { get; set; }

    public string name { get; set; } = "";

    public string comment { get; set; } = "";

    public bool is_enabled { get; set; } = true;

    public DateTime deadline { get; set; } = DateTime.Now;

    public DateTime started_at { get; set; } = DateTime.Now;

    public DateTime finished_at { get; set; } = DateTime.MinValue;
  }
}