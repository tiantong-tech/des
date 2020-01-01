using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBCore
{
  [Table("_migrations")]
  public class Migration
  {
    [Column("id")]
    public int Id { set; get; }

    [Column("batch_id")]
    public int BatchId { get; set; }

    [Column("file_name")]
    public string FileName { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

  }
}
