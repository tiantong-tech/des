using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("warehouses")]
  public class Warehouse : Entity
  {
    public int owner_user_id { get; set; }

    public string number { get; set; }

    public string name { get; set; } = "";

    public string address { get; set; } = "";

    public string comment { get; set; } = "";

    public bool is_enabled { get; set; } = true;

    public DateTime created_at { get; set; } = DateTime.Now;
  }
}
