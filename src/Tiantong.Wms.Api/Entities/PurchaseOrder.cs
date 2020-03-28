using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("purchase_orders")]
  public class PurchaseOrder : Entity
  {
    public int warehouse_id { get; set; }

    public string number { get; set; }

    public int operator_id { get; set; }

    public int applicant_id { get; set; }

    public int department_id { get; set; }

    public int supplier_id { get; set; }

    public int[] payment_ids { get; set; }

    public int[] purchase_item_ids { get; set; }

    public string status { get; set; } = "";

    public string comment { get; set; } = "";

    public DateTime due_time { get; set; } = DateTime.MinValue;

    public DateTime created_at { get; set; } = DateTime.Now;

    public DateTime finished_at { get; set; } = DateTime.MinValue;
  }
}
