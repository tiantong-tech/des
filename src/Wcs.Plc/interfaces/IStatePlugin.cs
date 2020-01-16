namespace Wcs.Plc
{
  public interface IStatePlugin
  {
    void Install<T>(IState<T> state);
  }
}
