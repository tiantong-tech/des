using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("keepers")]
  public class Keeper : Entity
  {
    public int warehouse_id { get; set; }

    public int user_id { get; set; }

    public string role { get; set; } = KeeperRoles.Admin;

  }
}
