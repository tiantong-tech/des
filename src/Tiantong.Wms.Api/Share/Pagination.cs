namespace Tiantong.Wms.Api
{
  public class Meta: IMeta
  {
    public int Page { get; set; }

    public int PageSize { get; set; }

    public int Total { get; set; }

    public Meta(int page, int pageSize, int total)
    {
      Page = page;
      PageSize = pageSize;
      Total = total;
    }
  }

  public class Pagination<TEntity, TKey>: Entities<TEntity, TKey>, IPagination<TEntity, TKey> where TEntity: IEntity<TKey>
  {
    public IMeta Meta { get; set; }
  }

  public class Pagination<TEntity>: Pagination<TEntity, int>, IPagination<TEntity>, IPagination<TEntity, int> where TEntity: IEntity<int>
  {

  }
}
