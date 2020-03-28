using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("locations")]
  public class Location : Entity
  {
    public int warehouse_id { get; set; }

    public int area_id { get; set; }

    public string number { get; set; }

    public string name { get; set; } = "";

    public string comment { get; set; } = "";

    public string total_area { get; set; } = "";

    public string total_volume { get; set; } = "";

    public bool is_enabled { get; set; } = true;

  }
}
