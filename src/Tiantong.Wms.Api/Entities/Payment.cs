using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("payments")]
  public class Payment : Entity
  {
    public double amount { get; set; }

    public string comment { get; set; } = "";

    public string order_type { get; set; }

    public string order_number { get; set; }

    public bool is_paid { get; set; } = false;

    public DateTime due_time { get; set; } = DateTime.MinValue;

    public DateTime paid_at { get; set; } = DateTime.MinValue;

  }
}
