using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yuchuan.IErp.Api
{
  [Table("users")]
  public class User
  {
    public int id { get; set; }

    public string name { get; set; }

    public string email { get; set; }

    public string mobile { get; set; }

    public string wechat_id { get; set; }

    public string password { get; set; }

    public bool is_enabled { get; set; }

    public bool is_verified { get; set; }

    public DateTime created_at { get; set; }
  }
}
