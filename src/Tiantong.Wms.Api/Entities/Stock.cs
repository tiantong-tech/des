using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("stocks")]
  public class Stock : Entity
  {
    public int warehouse_id { get; set; }

    public int good_id { get; set; }

    public int item_id { get; set; }

    public int area_id { get; set; }

    public int location_id { get; set; }

    public int quantity { get; set; }
  }
}
