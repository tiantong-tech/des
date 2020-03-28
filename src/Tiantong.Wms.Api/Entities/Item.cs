using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("items")]
  public class Item : Entity
  {
    public int warehouse_id { get; set; }

    public int good_id { get; set; }

    public string number { get; set; }

    public string name { get; set; }

    public string unit { get; set; }

    public bool is_enabled { get; set; } = true;

  }
}
