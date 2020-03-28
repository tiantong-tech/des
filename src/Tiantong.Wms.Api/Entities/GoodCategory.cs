using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("good_categories")]
  public class GoodCategory : Entity
  {
    public int warehouse_id { get; set; }

    public string number { get; set; }

    public string name { get; set; }

    public string comment { get; set; } = "";

    public bool is_enabled { get; set; } = true;

  }
}
