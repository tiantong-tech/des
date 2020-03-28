using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("purchase_order_items")]
  public class PurchaseOrderItem : Entity
  {
    public int good_id { get; set; }

    public int item_id { get; set; }

    public double price { get; set; }

    public int quantity { get; set; }

    public string delivery_cycle { get; set; }

    public string tax_number { get; set; }

    public string tax_name { get; set; }

    public string tax_specification { get; set; }

    public string tax_type { get; set; }

    public double tax_rate { get; set; }

    public int[] item_project_ids { get; set; }
  }
}
