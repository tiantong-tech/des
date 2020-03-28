namespace Tiantong.Wms.Api
{
  public interface IMeta
  {
    int Page { get; }

    int PageSize { get; }

    int Total { get; }
  }

  public interface IPagination<TEntity, TKey>: IEntities<TEntity, TKey> where TEntity: IEntity<TKey>
  {
    IMeta Meta { get; }
  }

  public interface IPagination<TEntity>: IPagination<TEntity, int> where TEntity: IEntity<int>
  {

  }

}
