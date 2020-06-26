using System.ComponentModel.DataAnnotations.Schema;

namespace Yuchuan.IErp.Api
{
  [Table("project_users")]
  public class ProjectUser
  {
    public int id { get; set; }

    public int project_id { get; set; }

    public int user_id { get; set; }

    public string role { get; set; }
  }
}
