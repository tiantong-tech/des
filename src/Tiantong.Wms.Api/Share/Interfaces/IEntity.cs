namespace Tiantong.Wms.Api
{
  public interface IEntity<T>
  {
    T GetKey();

    string GetStringKey();
  }
}
