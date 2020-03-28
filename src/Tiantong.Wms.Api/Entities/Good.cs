using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("goods")]
  public class Good : Entity
  {
    public int warehouse_id { get; set; }

    public int[] category_ids { get; set; } = new int[] {};

    public string number { get; set; }

    public string name { get; set; }

    public string comment { get; set; } = "";

    public bool is_enabled { get; set; } = true;

    public DateTime created_at { get; set; } = DateTime.Now;

    public int[] item_ids { get; set; } = new int[] {};

    public int[] stock_ids { get; set; } = new int[] {};

    // [ForeignKey("good_id")]
    // public List<Item> items { get; set; }

    // [ForeignKey("good_id")]
    // public List<Stock> stocks { get; set; }
  }
}
