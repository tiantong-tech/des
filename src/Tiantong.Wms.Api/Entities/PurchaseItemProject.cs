using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("purchase_item_projects")]
  public class PurchaseItemProject : Entity
  {
    public int project_id { get; set; }

    public int quantity { get; set; }

  }
}
