using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Tiantong.Wms.Api
{
  [Table("users")]
  public class User : Entity
  {
    public string email { get; set; }

    [JsonIgnore]
    public string password { get; set; }

    public string name { get; set; } = "";

    public string type { get; set; }

    public bool is_enabled { get; set; } = true;

    public DateTime created_at { get; set; } = DateTime.Now;

  }
}
