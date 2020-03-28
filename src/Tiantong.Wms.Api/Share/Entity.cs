using System.ComponentModel.DataAnnotations;

namespace Tiantong.Wms.Api
{
  public abstract class Entity<T>: IEntity<T>
  {
    public abstract T GetKey();

    public abstract string GetStringKey();
  }

  public class Entity: Entity<int>
  {
    [Key]
    public virtual int id { get; set; }

    public override int GetKey()
    {
      return this.id;
    }

    public override string GetStringKey()
    {
      return this.id.ToString();
    }
  }
}
