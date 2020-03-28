using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("stock_records")]
  public class StockRecord : Entity
  {
    public int stock_id { get; set; }

    public string order_type { get; set; }

    public string order_number { get; set; }

    public int quantity { get; set; }

    public DateTime created_at { get; set; } = DateTime.Now;
  }
}
